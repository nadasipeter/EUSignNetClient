using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using EUSignNetProject.Extensions;
using Microsoft.AspNetCore.Http;

namespace EUSignNetProject.Services.Logger
{
    /// <summary>
    /// Disposable class for logging how much time it takes until from instantiation until it is disposed using the static <see cref="Logger"/> class for logging.
    /// </summary>
    public sealed class StaticLoggerScope : ILoggerScope
    {
        private string parentExtraInfo;
        private string parentMemberName;
        private string parentSourceFilePath;
        private Stopwatch stopWatch = new Stopwatch();
        private ILoggerService Logger;
        private IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticLoggerScope"/> class.
        /// </summary>
        /// <param name="parentRequestId">The parent request id.</param>
        /// <param name="extraInfo">Extra information to be logged.</param>
        /// <param name="memberName">The name of the calling member.</param>
        /// <param name="sourceFilePath">The source file of the calling member.</param>
        public StaticLoggerScope(ILoggerService logger, IHttpContextAccessor httpContextAccessor, Guid parentRequestId, string extraInfo, string memberName, string sourceFilePath)
        {
            this.httpContextAccessor = httpContextAccessor;
            Logger = logger;
            RequestId = parentRequestId;
            parentExtraInfo = extraInfo ?? string.Empty;
            parentMemberName = memberName ?? string.Empty;
            parentSourceFilePath = sourceFilePath ?? string.Empty;
            Started(extraInfo, memberName, sourceFilePath);
            stopWatch.Start();
        }

        /// <summary>
        /// Gets the request id for the current logging context.
        /// </summary>
        public Guid RequestId { get; private set; }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Logs a certain message with Information classification.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogInfo(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            Logger.LogInfo(message, memberName, sourceFilePath);
        }

        /// <summary>
        /// Logs a certain message with Error classification.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            Logger.LogError(message, memberName, sourceFilePath);
        }

        /// <summary>
        /// Logs a certain message with Warning classification.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogWarning(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            Logger.LogWarning(message, memberName, sourceFilePath);
        }

        /// <summary>
        /// Logs a certain message and exception.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogException(string message, System.Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            Logger.LogException(message, ex, memberName, sourceFilePath);
        }

        /// <summary>
        /// Logs a certain exception.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogException(System.Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            Logger.LogException(ex, memberName, sourceFilePath);
        }

        /// <summary>
        /// Creates a disposable frame that logs how much time it takes between instantiation and disposing.
        /// </summary>
        /// <param name="extraInfo">Extra information to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <returns>The <see cref="ILoggerService"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public ILoggerScope Scope(string extraInfo, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            var scopeFactory = (ILoggerScopeFactory)httpContextAccessor.HttpContext.RequestServices.GetService(typeof(ILoggerScopeFactory));

            // TODO: add dummy scope?
            scopeFactory.Guard(() => scopeFactory);

            return scopeFactory.CreateScope(extraInfo, memberName, sourceFilePath);
        }

        /// <summary>
        /// Logs a specified "stated" message with Information classification.
        /// </summary>
        /// <param name="extraInfo">Extra information to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        private void Started(string extraInfo, string memberName, string sourceFilePath)
        {
            LogInfo(string.Format("started{0}", !string.IsNullOrWhiteSpace(extraInfo) ? " - " + extraInfo : string.Empty), memberName, sourceFilePath);
        }

        /// <summary>
        /// Logs a specified "finished" message with Information classification.
        /// </summary>
        /// <param name="extraInfo">Extra information to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        private void Finished(string extraInfo, string memberName, string sourceFilePath)
        {
            LogInfo(string.Format("finished{0}", !string.IsNullOrWhiteSpace(extraInfo) ? " - " + extraInfo : string.Empty), memberName, sourceFilePath);
        }

        /// <summary>
        /// Dispose pattern.
        /// </summary>
        /// <param name="disposing">If true managed resources are disposed as well as unmanaged.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                stopWatch.Stop();
                Finished(string.Format("took {0} ms{1}", stopWatch.ElapsedMilliseconds, !string.IsNullOrWhiteSpace(parentExtraInfo) ? " - " + parentExtraInfo : string.Empty), parentMemberName, parentSourceFilePath);
            }
        }
    }
}
