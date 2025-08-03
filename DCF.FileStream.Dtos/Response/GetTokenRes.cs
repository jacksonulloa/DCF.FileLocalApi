using DCF.FileStream.Entities.Generic;
namespace DCF.FileStream.Dtos.Response
{
    public class GetTokenRes:BaseResponse
    {
        public string Token { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
    }
}
