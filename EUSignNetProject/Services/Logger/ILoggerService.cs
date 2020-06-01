using System;
using System.Runtime.CompilerServices;

namespace EUSignNetProject.Services.Logger
{
    public interface ILoggerService
    {
        /// <summary>
        /// Gets the request id for the current logging context.
        /// </summary>
        Guid RequestId { get; }

        /// <summary>
        /// Logs a certain message with Information classification.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        void LogInfo(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");

        /// <summary>
        /// Logs a certain message with Error classification.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        void LogError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");

        /// <summary>
        /// Logs a certain message with Warning classification.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        void LogWarning(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");

        /// <summary>
        /// Logs a certain message and exception.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        void LogException(string message, Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");

        /// <summary>
        /// Logs a certain exception.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        void LogException(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");

        /// <summary>
        /// Creates a disposable frame that logs how much time it takes between instantiation and disposing.
        /// </summary>
        /// <param name="extraInfo">Extra information to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <returns>The <see cref="ILoggerService"/> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        ILoggerScope Scope(string extraInfo, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");
    }
}