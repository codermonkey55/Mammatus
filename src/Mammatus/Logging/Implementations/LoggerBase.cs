using Mammatus.Core.IoC;
using System;
using System.Runtime.CompilerServices;

namespace Mammatus.Logging.Implementations
{
    public abstract class LoggerBase : ILogger
    {
        protected LoggerBase()
        {
            Config = new LoggerConfiguration();
        }

        #region Properties
        public LoggerConfiguration Config { get; set; }

        protected ILoggerFormatter Formatter => Kernel.Resolve<ILoggerFormatter>();
        #endregion

        public ILogger Debug(string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledDebug)
                Write(LogType.Debug, message, args);

            return this;
        }

        public ILogger Debug(LoggerInfo info, string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledDebug)
                Write(info, LogType.Debug, message, args);

            return this;
        }

        public ILogger Error(string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledError)
                Write(LogType.Error, message, args);

            return this;
        }

        public ILogger Error(LoggerInfo info, string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledError)
                Write(info, LogType.Error, message, args);

            return this;
        }

        public ILogger Exception(Exception exception)
        {
            if (Config.Enabled && Config.EnabledException)
                Write(LogType.Exception, exception.Message);

            return this;
        }

        public ILogger Exception(string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledException)
                Write(LogType.Exception, message, args);

            return this;
        }

        public ILogger Exception(LoggerInfo info, string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledException)
                Write(info, LogType.Exception, message, args);

            return this;
        }

        public ILogger Info(string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledInfo)
                Write(LogType.Info, message, args);

            return this;
        }

        public ILogger Info(LoggerInfo info, string message, params object[] args)
        {
            if (Config.Enabled && Config.EnabledInfo)
                Write(info, LogType.Info, message, args);

            return this;
        }

        public ILogger Log(string message, params object[] args)
        {
            if (Config.Enabled)
                Write(LogType.Info, message, args);

            return this;
        }

        public ILogger Log(LoggerInfo info, string message, params object[] args)
        {
            if (Config.Enabled)
                Write(info, LogType.Info, message, args);

            return this;
        }

        public ILogger Log(LogType type, string message, params object[] args)
        {
            if (Config.Enabled)
                Write(type, message, args);

            return this;
        }

        public ILogger Log(LoggerInfo info, LogType type, string message, params object[] args)
        {
            if (Config.Enabled)
                Write(info, type, message, args);

            return this;
        }

        public LoggerInfo GetInfo([CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            return new LoggerInfo
            {
                MemberName = memberName,
                FileName = fileName,
                LineNumber = lineNumber
            };
        }

        protected abstract void Write(LogType type, string message, params object[] args);

        protected abstract void Write(LoggerInfo info, LogType type, string message, params object[] args);
    }
}
