using DCF.FileStream.Entities.Generic;
using DCF.FileStream.Services.Interfaces;
using DCF.FileStream.Utils.Interfaces;
using Microsoft.Extensions.Options;

namespace DCF.FileStream.Services.Implementations
{
    public class DependencyProviderService(IOptions<AppSettings> _aps, IToolsService _tos, ILogService _los) : IDependencyProviderService
    {
        public AppSettings aps { get; } = _aps.Value;
        public IToolsService tos { get; } = _tos;
        public ILogService los { get; } = _los;
    }
}
