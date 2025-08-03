using DCF.FileStream.Entities.Generic;

namespace DCF.FileStream.Utils.Interfaces
{
    public interface IToolsService
    {
        string CalcularDuracion(DateTime fechaIni, DateTime fechaFin);
        string DesencriptarString(string key, string textoCifrado);
        string EncriptarString(string key, string texto);
        DateTime? ObtenerFechaExpiracionToken(string token);
        void SetErrorResponse(BaseResponse _response, Exception exc);
        void SetFinalResponse(BaseResponse _response);
        void SetResponse(BaseResponse _response, Exception exc);
    }
}