using System;
using System.Runtime.CompilerServices;

namespace Mammatus.Logging
{
    public interface ILogger
    {
        LoggerConfiguration Config { get; }

        LoggerInfo GetInfo([CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);

        ILogger Debug(string message, params object[] args);

        ILogger Debug(LoggerInfo info, string message, params object[] args);

        ILogger Error(string message, params object[] args);

        ILogger Error(LoggerInfo info, string message, params object[] args);

        ILogger Exception(string message, params object[] args);

        ILogger Exception(LoggerInfo info, string message, params object[] args);

        ILogger Info(string message, params object[] args);

        ILogger Info(LoggerInfo info, string message, params object[] args);

        ILogger Log(string message, params object[] args);

        ILogger Log(LoggerInfo info, string message, params object[] args);

        ILogger Log(LogType type, string message, params object[] args);

        ILogger Log(LoggerInfo info, LogType type, string message, params object[] args);

        ILogger Exception(Exception exception);
    }
}
