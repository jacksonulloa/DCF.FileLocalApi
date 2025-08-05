using DCF.FileStream.Dtos.Request;
using DCF.FileStream.Dtos.Response;
using DCF.FileStream.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DCF.FileStream.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController(IFileService _fis, IDependencyProviderService _dps) : ControllerBase
    {
        private readonly IFileService fis = _fis;
        private readonly IDependencyProviderService dps = _dps;
        [Authorize(Policy = "credentialtipo2")]
        [HttpPost("validar")]
        public async Task<IActionResult> ValidarArchivoAsync(SearchFileReq req)
        {
            var response = await fis.ValidarArchivo(req);
            dps.los.WriteResumeLog("FileController => ValidarArchivoAsync", req, response, dps.aps.GlobalConfig.SystemName);
            return response is not null ? Ok(response) : BadRequest(response);
        }
        [Authorize(Policy = "credentialtipo1")]
        [HttpPost("descargar")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(DownFileRes), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DownFileRes), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DescargarArchivoAsync(DownFileReq req)
        {
            var resultado = await fis.DescargarArchivo(req);
            //dps.los.WriteResumeLog("FileController => DescargarArchivoAsync", req, resultado, dps.aps.GlobalConfig.SystemName);
            if (resultado.CodResp == "99" || resultado.CodResp == "22")
            {
                return NotFound(new
                {
                    codigo = resultado.CodResp,
                    descripcion = resultado.DesResp
                });
            }            
            return File(resultado.Stream, resultado.ContentType, resultado.NombreArchivo);
        }
    }
}
