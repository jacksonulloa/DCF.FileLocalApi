using DCF.FileStream.Entities.Generic;

namespace DCF.FileStream.Dtos.Response
{
    public class DownFileRes : BaseResponse
    {
        public Stream Stream { get; set; } = default!;
        public string ContentType { get; set; } = "application/octet-stream";
        public string NombreArchivo { get; set; } = default!;
    }
}
