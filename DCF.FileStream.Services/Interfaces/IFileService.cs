using DCF.FileStream.Dtos.Request;
using DCF.FileStream.Dtos.Response;

namespace DCF.FileStream.Services.Interfaces
{
    public interface IFileService
    {
        Task<DownFileRes> DescargarArchivo(SearchFileReq request);
        Task<SearchFileRes> ValidarArchivo(SearchFileReq request);
    }
}