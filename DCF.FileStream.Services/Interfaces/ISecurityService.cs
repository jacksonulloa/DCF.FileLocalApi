using DCF.FileStream.Dtos.Request;
using DCF.FileStream.Dtos.Response;

namespace DCF.FileStream.Services.Interfaces
{
    public interface ISecurityService
    {
        GetTokenRes GetToken(GetTokenReq _req);
    }
}