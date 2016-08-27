using System;

namespace Mammatus.Utility.Logging
{
    public static class LoggerManager
    {
        public static Func<Type, ILogger> GetLogger = type => new EmptyLogger();
    }
}