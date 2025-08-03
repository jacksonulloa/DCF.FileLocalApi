using DCF.FileStream.Entities.Generic;
using DCF.FileStream.Utils.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DCF.FileStream.Utils.Implementations
{
    public class LogService(ILogger<LogService> Los, IToolsService Tos) : ILogService
    {
        public readonly ILogger<LogService> _Los = Los;
        public readonly IToolsService _Tos = Tos;
        public void LogDynamic(string codResp, string message, params object[] args)
        {
            switch (codResp)
            {
                case "00":
                case "01":
                case "02":
                case "03":
                case "04":
                case "16":
                case "22":
                    _Los.LogInformation(message, args);
                    break;
                case "80":
                case "81":
                case "88":
                case "89":
                case "99":
                    _Los.LogError(message, args);
                    break;
                default:
                    throw new Exception($"LogDynamic not supported for {codResp}");
            }
        }
        public void WriteResumeLog(string _descComponente, BaseResponse _response, string SystemName)
        {
            LogDynamic(_response.CodResp, $"{SystemName} {_descComponente}");
            LogDynamic(_response.CodResp, $"{SystemName} Inicio => {_response.StartExec:dd-MM-yyyy hh:mm:ss tt} Fin => {_response.EndExec:dd-MM-yyyy hh:mm:ss ttt}");
            LogDynamic(_response.CodResp, $"{SystemName} Resumen => {_response.CodResp} - {_response.DesResp} [Time Exec: {_response.Duration}]");
        }
        public void WriteResumeLog<T1, T2>(string _descComponente, T1 _request, T2 _response, string SystemName)
            where T2 : BaseResponse
        {
            LogDynamic(_response.CodResp, $"{SystemName} {_descComponente}");
            LogDynamic(_response.CodResp, $"{SystemName} Json Request {JsonSerializer.Serialize(_request)}");
            LogDynamic(_response.CodResp, $"{SystemName} Json Response {JsonSerializer.Serialize(_response)}");
            LogDynamic(_response.CodResp, $"{SystemName} Inicio => {_response.StartExec:dd-MM-yyyy hh:mm:ss tt} Fin => {_response.EndExec:dd-MM-yyyy hh:mm:ss tt}");
            LogDynamic(_response.CodResp, $"{SystemName} Resumen => {_response.CodResp} - {_response.DesResp} [Time Exec: {_response.Duration}]");
        }
    }
}
