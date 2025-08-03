using DCF.FileStream.Entities.Generic;
using DCF.FileStream.Utils.Interfaces;

namespace DCF.FileStream.Services.Interfaces
{
    public interface IDependencyProviderService
    {
        AppSettings aps { get; }
        IToolsService tos { get; }
        ILogService los { get; }
    }
}