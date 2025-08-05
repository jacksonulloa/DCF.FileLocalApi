using DCF.FileStream.Dtos.Request;
using DCF.FileStream.Dtos.Response;
using DCF.FileStream.Services.Interfaces;

namespace DCF.FileStream.Services.Implementations
{
    public class FileService(IDependencyProviderService _dps) : IFileService
    {
        private readonly IDependencyProviderService dps = _dps;
        public async Task<SearchFileRes> ValidarArchivo(SearchFileReq request)
        {
            SearchFileRes response = new() { CodResp = "00", DesResp = "Ok", StartExec = DateTime.Now };
            string pathFileComplete = string.Empty;
            try
            {
                string rutaparcial = request.path.StartsWith("~/") || request.path.StartsWith("~\\") ? request.path.Substring(2) : request.path;
                rutaparcial = rutaparcial.Replace("/", "\\"); // <-- Esta línea es clave
                pathFileComplete = Path.Combine(dps.aps.FileConfig.RootPathLocal, rutaparcial);
                string folderPath = Path.GetDirectoryName(pathFileComplete) ?? "";
                bool existe = Directory.Exists(folderPath);
                
                string fileName = Path.GetFileName(pathFileComplete);
                response.CodResp = File.Exists(pathFileComplete) ? "00" : "22";
                response.DesResp = response.CodResp.Equals("00") ? "Found" : "Not found";
            }
            catch (Exception ex)
            {
                dps.tos.SetErrorResponse(response, ex);
            }
            finally
            {
                dps.tos.SetFinalResponse(response);
            }
            return response;
        }

        public async Task<DownFileRes> DescargarArchivo(DownFileReq request)
        {
            DownFileRes response = new() { CodResp = "00", DesResp = "Ok", StartExec = DateTime.Now };
            DownFileRes temporal = new();
            string pathFileComplete = string.Empty;
            try
            {
                string rutaparcial = request.path.StartsWith("~/") || request.path.StartsWith("~\\") ? request.path.Substring(2) : request.path;
                rutaparcial = rutaparcial.Replace("/", "\\"); // <-- Esta línea es clave
                pathFileComplete = Path.Combine(dps.aps.FileConfig.RootPathLocal, rutaparcial);

                var streamObj = new System.IO.FileStream(
                                        pathFileComplete,
                                        FileMode.Open,
                                        FileAccess.Read,
                                        FileShare.Read,
                                        bufferSize: 81920, // estándar para streams
                                        options: FileOptions.Asynchronous);
                temporal = new()
                {
                    Stream = streamObj,
                    ContentType = request.ContentType,
                    NombreArchivo = Path.GetFileName(pathFileComplete)
                };
            }
            catch (Exception ex)
            {
                dps.tos.SetErrorResponse(response, ex);
            }
            finally
            {
                dps.tos.SetFinalResponse(response);
                response.Stream = temporal.Stream;
                response.ContentType = temporal.ContentType;
                response.NombreArchivo = temporal.NombreArchivo;
            }
            return response;
        }
    }
}
