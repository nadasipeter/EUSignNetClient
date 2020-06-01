using System;
using Microsoft.AspNetCore.Http;

namespace EUSignNetProject.Services.Logger
{
    /// <summary>
    /// Logger scope factory class that creates a <see cref="StaticLoggerScope"/>.
    /// </summary>
    public sealed class StaticLoggerScopeFactory : ILoggerScopeFactory
    {
        private readonly ILoggerService loggerService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public StaticLoggerScopeFactory(ILoggerService loggerService, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.loggerService = loggerService;
        }
        /// <summary>
        /// Creates a <see cref="StaticLoggerScope"/> object.
        /// </summary>        
        /// <param name="extraInfo">Extra information to be logged.</param>
        /// <param name="memberName">The name of the calling member.</param>
        /// <param name="sourceFilePath">The source file of the calling member.</param>
        /// <returns>The <see cref="ILoggerScope"/> object.</returns>
        public ILoggerScope CreateScope(string extraInfo, string memberName, string sourceFilePath)
        {
            return new StaticLoggerScope(loggerService, httpContextAccessor, loggerService == null ? System.Guid.Empty : loggerService.RequestId, extraInfo, memberName, sourceFilePath);
        }
    }
}
