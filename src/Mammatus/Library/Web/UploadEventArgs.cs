using System;

namespace Mammatus.Library.Web
{
    public class UploadEventArgs : EventArgs
    {
        public int BytesSent { get; set; }

        public int TotalBytes { get; set; }
    }

}