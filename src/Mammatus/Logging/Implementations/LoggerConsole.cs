using System;

namespace Mammatus.Logging.Implementations
{
    public class LoggerConsole : LoggerBase
    {
        protected override void Write(LogType type, string message, params object[] args)
        {
            Console.WriteLine(Formatter.Format(type, message, args));
        }

        protected override void Write(LoggerInfo info, LogType type, string message, params object[] args)
        {
            Console.WriteLine(Formatter.Format(info, type, message, args));
        }
    }
}
