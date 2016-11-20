namespace Mammatus.Logging.Implementations
{
    public class LoggerEmpty : LoggerBase
    {
        protected override void Write(LogType type, string message, params object[] args)
        {
        }

        protected override void Write(LoggerInfo info, LogType type, string message, params object[] args)
        {
        }
    }
}
