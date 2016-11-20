using System;
using System.Text;

namespace Mammatus.Logging.Implementations
{
    public class LoggerFormatterStandard : ILoggerFormatter
    {
        public string Format(LogType type, string message, params object[] args)
        {
            string messageComplete = string.Format(message, args);
            return string.Format("{0} {1}: \t {2}", DateTime.Now, type, messageComplete);
        }

        public string Format(LoggerInfo info, LogType type, string message, params object[] args)
        {
            StringBuilder result = new StringBuilder();

            string messageComplete = string.Format(message, args);
            result.AppendLine(string.Format("{0} {1}: \t {2}", DateTime.Now, type, messageComplete));

            if (info != null)
            {
                result.AppendLine(string.Format("\tFile:\t{0}", info.FileName));
                result.AppendLine(string.Format("\tMethod:\t{0}", info.MemberName));
                result.AppendLine(string.Format("\tLine:\t{0}", info.LineNumber));
            }

            return result.ToString();
        }
    }
}
