using System;
namespace EUSignNetProject.Services.Logger
{
    public interface ILoggerScopeFactory
    {
        ILoggerScope CreateScope(ILoggerService parentService, string extraInfo, string memberName, string sourceFilePath);
    }
}
