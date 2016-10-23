using System;

namespace Mammatus.Utility.Logging
{
    public static class LoggingManager
    {
        public static Func<Type, ILogger> GetLogger = type => new EmptyLogger();
    }
}