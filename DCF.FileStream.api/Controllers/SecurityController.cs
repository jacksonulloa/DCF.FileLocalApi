using DCF.FileStream.Dtos.Request;
using DCF.FileStream.Dtos.Response;
using DCF.FileStream.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DCF.FileStream.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController(IDependencyProviderService _dps, ISecurityService ses) : ControllerBase
    {
        private readonly IDependencyProviderService dps = _dps;
        private readonly ISecurityService Ses = ses;
        [ProducesResponseType(typeof(GetTokenRes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("getToken")]
        public async Task<IActionResult> CreateToken(GetTokenReq _login)
        {
            GetTokenRes response = Ses.GetToken(_login);
            dps.los.WriteResumeLog("SecurityController => CreateToken", _login, response, dps.aps.GlobalConfig.SystemName);
            return response is not null ? Ok(response) : BadRequest(response);
        }
    }
}
