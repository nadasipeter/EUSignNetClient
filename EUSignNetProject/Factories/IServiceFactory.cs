using System;
namespace EUSignNetProject.Factories
{
    public interface IServiceFactory
    {
        IRemoteSignService CreateRemoteSignService(IServiceContext context);
    }
}
