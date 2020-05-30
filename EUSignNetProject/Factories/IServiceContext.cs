using System;
using EUSignNetProject.Services.Logger;

namespace EUSignNetProject.Factories
{
    public interface IServiceContext
    {
        public Guid TenantId { get; }
        public string CurrentUserIdentity { get; }
        public ILoggerService Logger { get; }
        public IServiceRequest ServiceRequest { get; set; }
    }
}