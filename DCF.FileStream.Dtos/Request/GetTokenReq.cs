namespace DCF.FileStream.Dtos.Request
{
    public class GetTokenReq
    {
        public string UserType { get; set; } = string.Empty;
        public string UserProfile { get; set; } = string.Empty;
        public string UserPass { get; set; } = string.Empty;
    }
}
