using System.IO;

namespace Mammatus.Reactive.Utils.IO.EventArgs
{
    public class RxFileSystemEventArgs
    {
        public WatcherChangeTypes ChangeType { get; }

        public string FullPath { get; }

        public string OldFullPath { get; }

        public string Directory { get; }

        public RxFileSystemEventArgs
            (WatcherChangeTypes changeType, string fullPath, string oldFullPath, string directory)
        {
            this.ChangeType = changeType;
            this.FullPath = fullPath;
            this.OldFullPath = oldFullPath;
            this.Directory = directory;
        }
    }
}