using System;
using log4net.Appender;

namespace EUSignNetProject.Services.Logger
{
    public sealed class Log4NetHeaderOnceRollingFileAppender : RollingFileAppender
    {
        protected override void WriteHeader()
        {
            if (LockingModel.AcquireLock().Length == 0)
            {
                base.WriteHeader();
            }
        }
    }
}
