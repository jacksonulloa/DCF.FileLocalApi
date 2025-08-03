using DCF.FileStream.Dtos.Request;
using DCF.FileStream.Dtos.Response;
using DCF.FileStream.Entities.Generic;
using DCF.FileStream.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;

namespace DCF.FileStream.Services.Implementations
{
    public class SecurityService(IDependencyProviderService _dps) : ISecurityService
    {
        private readonly IDependencyProviderService dps = _dps;

        public GetTokenRes GetToken(GetTokenReq _req)
        {
            GetTokenRes response = new();
            try
            {
                string Key = "4sb4nc-T1-734m-1ng3r14SW";
                string role = dps.tos.EncriptarString(Key, _req.UserType);
                string identifier = dps.tos.EncriptarString(Key, _req.UserProfile);
                string secret = dps.tos.EncriptarString(Key, _req.UserPass);
                List<CredentialConfig> listaTemp = dps.aps.CredentialsConfig.Where(x => x.Role == role && x.Identifier == identifier && x.Secret == secret).ToList();
                response.CodResp = listaTemp.Count != 0 ? "00" : "99";
                response.DesResp = listaTemp.Count != 0 ? "Ok" : "Wrong credentials";
                if (listaTemp.Count != 0)
                {
                    int minutes = int.Parse(dps.aps.Jwt.MinutesFactor);
                    int hours = int.Parse(dps.aps.Jwt.HoursFactor);
                    int days = int.Parse(dps.aps.Jwt.DaysFactor);
                    int tokenTime = minutes * hours * days;
                    response.StartExec = DateTime.Now;
                    var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dps.aps.Jwt.SecretKey)) ?? default;
                    var credentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                    {
                        new("users", _req.UserType) // este será "admin", "superadmin", etc.
                    };
                    var token = new JwtSecurityToken(
                        issuer: dps.aps.Jwt.Issuer,
                        audience: dps.aps.Jwt.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(tokenTime),
                        signingCredentials: credentials
                    );
                    response.Token = new JwtSecurityTokenHandler().WriteToken(token);
                    response.ExpirationDate = response.CodResp.Equals("00") ?
                                                $"{dps.tos.ObtenerFechaExpiracionToken(response.Token):dd/MM/yyyy-hh:mm:ss tt}" :
                                                "Problemas con el token";
                }
            }
            catch (Exception ex)
            {
                response.CodResp = "99";
                response.DesResp = ex.Message;
            }
            finally
            {
                response.EndExec = DateTime.Now;
                response.Duration = dps.tos.CalcularDuracion(response.StartExec, response.EndExec);
            }
            return response;
        }
    }
}
