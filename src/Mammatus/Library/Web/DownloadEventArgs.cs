using System;

namespace Mammatus.Library.Web
{
    public class DownloadEventArgs : EventArgs
    {
        public int BytesReceived { get; set; }

        public int TotalBytes { get; set; }

        public byte[] ReceivedData { get; set; }
    }

}