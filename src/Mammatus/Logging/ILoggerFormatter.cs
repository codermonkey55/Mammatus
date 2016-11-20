namespace Mammatus.Logging
{
    public interface ILoggerFormatter
    {
        string Format(LogType type, string message, params object[] args);

        string Format(LoggerInfo info, LogType type, string message, params object[] args);
    }
}
