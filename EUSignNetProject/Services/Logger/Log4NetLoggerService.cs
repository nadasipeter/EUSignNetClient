using System;
using EUSignNetProject.Factories;
using log4net.Core;
using log4net.Util;
using log4net.Config;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Collections;
using EUSignNetProject.Extensions;
using log4net;

namespace EUSignNetProject.Services.Logger
{
    public class Log4NetLoggerService : ILoggerService
    {
        public IHttpContextAccessor httpContextAccessor;

        private static readonly Type LogDeclaringType;
        private static readonly log4net.ILog Log;
        private static readonly Level LevelInfo;
        private static readonly Level LevelWarning;
        private static readonly Level LevelError;
        private static readonly string Area;
        private static readonly string EventId;

        /// <summary>
        /// Initializes static members of the <see cref="Log4NetLoggerService"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Better to have all static field initialized in one place.")]
        static Log4NetLoggerService()
        {
            LogDeclaringType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
            Log = GetLogger(typeof(Log4NetLoggerService));

            var repository = Log.Logger.Repository;
            log4net.Config.XmlConfigurator.Configure(repository);
            LevelMap levelMap = repository.LevelMap;

            levelMap.Add("Verbose", levelMap.LookupWithDefault(Level.Info).Value);
            levelMap.Add("Medium", levelMap.LookupWithDefault(Level.Warn).Value);
            levelMap.Add("High", levelMap.LookupWithDefault(Level.Error).Value);

            LevelInfo = levelMap["Verbose"];
            LevelWarning = levelMap["Medium"];
            LevelError = levelMap["High"];
            Area = "EUSignNet";
            EventId = "00000";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLoggerService"/> class.
        /// </summary>
        public Log4NetLoggerService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the request id for the current logging context.
        /// </summary>
        public Guid RequestId => GetRequestId();

        /// <summary>
        /// Logs a certain message with Error classification.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            LogData(LevelError, message, RequestId, Area, memberName, EventId, DateTime.UtcNow);
        }

        /// <summary>
        /// Logs a certain message and exception.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogException(string message, Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            message = message ?? "Exception occurred.";
            LogError(string.Format("{0} Exception name:'{1}', Exception message:'{2}' , Stack:'{3}'", message, ex == null ? "N/A" : ex.GetType().Name, ex == null ? "N/A" : ex.Message, ex == null ? "N/A" : ex.StackTrace), memberName, sourceFilePath);
            var aggregatedException = ex as AggregateException;
            if (aggregatedException != null)
            {
                var flatten = aggregatedException.Flatten();
                if (flatten != null)
                {
                    foreach (var innnerException in flatten.InnerExceptions)
                    {
                        LogError(string.Format("{0} Exception name:'{1}', Exception message:'{2}' , Stack:'{3}'", message, innnerException == null ? "N/A" : innnerException.GetType().Name, innnerException == null ? "N/A" : innnerException.Message, innnerException == null ? "N/A" : innnerException.StackTrace), memberName, sourceFilePath);
                    }
                }
            }
            else
            {
                try
                {
                    if (ex != null && ex.InnerException != null)
                    {
                        LogException(message, ex.InnerException, memberName, sourceFilePath);
                    }
                }
                catch
                {
                    LogError("Swallowed exception during getting inner exceptions.", memberName, sourceFilePath);
                }
            }
        }

        /// <summary>
        /// Logs a certain exception.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="memberName">The name of the calling member. If not set .Net framework will set it at compile time.</param>
        /// <param name="sourceFilePath">The source file of the calling member. If not set .Net framework will set it at compile time.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = ".Net restriction")]
        public void LogException(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            LogException(string.Empty, ex, memberName, sourceFilePath);
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
            LogData(LevelInfo, message, RequestId, Area, memberName, EventId, DateTime.UtcNow);
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
            LogData(LevelWarning, message, RequestId, Area, memberName, EventId, DateTime.UtcNow);
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
            var service = ServiceLocator.Resolve<ILoggerService>();
            var scopeFactory = ServiceLocator.Resolve<ILoggerScopeFactory>();

            scopeFactory.Guard(() => scopeFactory);

            return scopeFactory.CreateScope(service, extraInfo, memberName, sourceFilePath);
        }

        /// <summary>
        /// Parses the input string value to a Guid.
        /// </summary>
        /// <param name="rawValue">The raw string containing the id.</param>
        /// <returns>The parsed Guid or a Guid.Empty if the parsing failed.</returns>
        protected static Guid ParseCorrelationIdSafe(string rawValue)
        {
            Guid requestId;
            if (string.IsNullOrEmpty(rawValue) || !Guid.TryParse(rawValue, out requestId))
            {
                requestId = Guid.Empty;
            }

            return requestId;
        }

        /// <summary>
        /// Retrieves the SharePoint request id from the request.
        /// </summary>
        /// <returns>The SharePoint request id from the request or an empty <see cref="Guid"/> if not set.</returns>
        protected virtual Guid GetRequestId()
        {
            var requestId = Guid.Empty;

            if (requestId == Guid.Empty && httpContextAccessor != null && httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Response != null)
            {
                requestId = ParseCorrelationIdSafe(httpContextAccessor.HttpContext.Response.Headers["X-SPCorrelationId"]);
            }

            if (requestId == Guid.Empty && httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Request != null)
            {
                requestId = ParseCorrelationIdSafe(httpContextAccessor.HttpContext.Request.Headers["SPRequestGuid"]);

                if (requestId == Guid.Empty)
                {
                    try
                    {
                        requestId = ParseCorrelationIdSafe(httpContextAccessor.HttpContext.Request.Form["SPCorrelationId"]);
                    }
                    catch { }
                }

                if (requestId == Guid.Empty)
                {
                    requestId = ParseCorrelationIdSafe(httpContextAccessor.HttpContext.Request.Query["SPRequestGuid"]);
                }

                if (requestId == Guid.Empty)
                {
                    requestId = ParseCorrelationIdSafe(httpContextAccessor.HttpContext.Request.Query["SPCorrelationId"]);
                }
            }

            return requestId;
        }

        /// <summary>
        /// Logs a specific message with Log4Net.
        /// </summary>
        /// <param name="level">The level of the entry.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="correlation">The correlation id to log.</param>
        /// <param name="area">The area to log.</param>
        /// <param name="category">The category to log.</param>
        /// <param name="eventId">The event id to log.</param>
        /// <param name="when">The time to log.</param>
        private static void LogData(Level level, string message, Guid correlation, string area, string category, string eventId, DateTime when)
        {
            IDictionary props = new PropertiesDictionary();
            props.Add("Area", area);
            props.Add("Category", category);
            props.Add("EventID", eventId);

            props.Add("Correlation", correlation);

            var data = new LoggingEventData
            {
                TimeStampUtc = when,
                LoggerName = Log.Logger.Name,
                Level = level,
                Message = FilterMessage(message),
                ExceptionString = string.Empty,
                Properties = (PropertiesDictionary)props
            };

            var loggingEvent = new LoggingEvent(LogDeclaringType, Log.Logger.Repository, data);
            Log.Logger.Log(loggingEvent);
        }

        /// <summary>
        /// Removes special characters from a <see cref="string"/> so that ULS can parse it properly.
        /// </summary>
        /// <param name="message">The <see cref="string"/> that needs to be filtered.</param>
        /// <returns>The filtered <see cref="string"/>.</returns>
        private static string FilterMessage(string message)
        {
            message = message ?? string.Empty;
            message = Regex.Replace(message, "[\r\n\t]", string.Empty, RegexOptions.Compiled);
            return message;
        }

        private static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}
