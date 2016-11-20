using System.IO;
using System.Threading.Tasks;

namespace Mammatus.Logging.Implementations
{
    public class LoggerFile : LoggerBase
    {
        static readonly object LockObject = new object();

        public string FileName { get; set; }

        public LoggerFile(string fileName)
        {
            FileName = fileName;

            FileName = fileName;
            var path = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        protected override void Write(LogType type, string message, params object[] args)
        {
            Task.Factory.StartNew(() =>
            {
                lock (LockObject)
                {
                    using (StreamWriter write = File.AppendText(FileName))
                    {
                        write.WriteLine(Formatter.Format(type, message, args));
                    }
                }
            });
        }

        protected override void Write(LoggerInfo info, LogType type, string message, params object[] args)
        {
            Task.Factory.StartNew(() =>
            {
                lock (LockObject)
                {
                    using (StreamWriter write = File.AppendText(FileName))
                    {
                        write.WriteLine(Formatter.Format(info, type, message, args));
                    }
                }
            });
        }
    }
}
