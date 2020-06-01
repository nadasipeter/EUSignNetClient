using System;
namespace EUSignNetProject.Services.Logger
{
    public interface ILoggerScopeFactory
    {
        ILoggerScope CreateScope(string extraInfo, string memberName, string sourceFilePath);
    }
}
