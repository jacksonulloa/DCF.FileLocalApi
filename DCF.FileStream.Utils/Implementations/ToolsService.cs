using DCF.FileStream.Entities.Generic;
using DCF.FileStream.Utils.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace DCF.FileStream.Utils.Implementations
{
    public class ToolsService : IToolsService
    {
        public string CalcularDuracion(DateTime fechaIni, DateTime fechaFin)
        {
            TimeSpan duracion = fechaFin - fechaIni;
            double segundosTotales = (duracion.TotalMilliseconds) / 1000;
            return $"{segundosTotales:F5} seg";
        }
        public string EncriptarString(string key, string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(texto);
                }
                array = memoryStream.ToArray();
            }
            return Convert.ToBase64String(array);
        }
        public string DesencriptarString(string key, string textoCifrado)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(textoCifrado);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                return streamReader.ReadToEnd();
            }
        }
        public DateTime? ObtenerFechaExpiracionToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            return jwtToken.ValidTo.ToLocalTime();
        }
        public void SetErrorResponse(BaseResponse _response, Exception exc)
        {
            _response.CodResp = "99";
            _response.DesResp = $"Error => {exc.Message}";
        }
        public void SetResponse(BaseResponse _response, Exception exc)
        {
            _response.CodResp = "99";
            _response.DesResp = $"Error => {exc.Message}";
        }
        public void SetFinalResponse(BaseResponse _response)
        {
            _response.EndExec = DateTime.Now;
            _response.Duration = CalcularDuracion(_response.StartExec, _response.EndExec);
        }
    }
}
