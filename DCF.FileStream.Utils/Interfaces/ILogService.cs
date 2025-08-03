using DCF.FileStream.Entities.Generic;

namespace DCF.FileStream.Utils.Interfaces
{
    public interface ILogService
    {
        void LogDynamic(string codResp, string message, params object[] args);
        void WriteResumeLog(string _descComponente, BaseResponse _response, string SystemName);
        void WriteResumeLog<T1, T2>(string _descComponente, T1 _request, T2 _response, string SystemName) where T2 : BaseResponse;
    }
}