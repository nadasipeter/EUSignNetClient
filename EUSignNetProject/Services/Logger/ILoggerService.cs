using System.Runtime.CompilerServices;

namespace EUSignNetProject.Services.Logger
{
    public interface ILoggerService
    {
        void LogInfo(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");
    }
}