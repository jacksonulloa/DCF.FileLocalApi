namespace DCF.FileStream.Dtos.Request
{
    public class DownFileReq : SearchFileReq
    {
        public string ContentType { get; set; } = string.Empty;
    }
}
