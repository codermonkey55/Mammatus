using System;

namespace Mammatus.Utilities.Logging
{
    public static class LoggerManager
    {
        public static Func<Type, ILogger> GetLogger = type => new EmptyLogger();
    }
}