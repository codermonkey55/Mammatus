
using System.IO;
using System.Text;

namespace Mammatus.Library.Web
{
    using System;

    public class MultipartForm
    {
        private Encoding _encoding;
        private readonly MemoryStream _ms;
        private readonly string _boundary;
        private byte[] _formData;

        public byte[] FormData
        {
            get
            {
                if (_formData == null)
                {
                    byte[] buffer = _encoding.GetBytes("--" + _boundary + "--\r\n");
                    _ms.Write(buffer, 0, buffer.Length);
                    _formData = _ms.ToArray();
                }
                return _formData;
            }
        }

        public string ContentType
        {
            get { return $"multipart/form-data; boundary={_boundary}"; }
        }

        public Encoding StringEncoding
        {
            set { _encoding = value; }
            get { return _encoding; }
        }

        public MultipartForm()
        {
            _boundary = $"--{Guid.NewGuid()}--";
            _ms = new MemoryStream();
            _encoding = Encoding.Default;
        }

        public void AddFlie(string name, string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileData = new byte[fs.Length];
                fs.Read(fileData, 0, fileData.Length);
                AddFlie(name, Path.GetFileName(filename), fileData, fileData.Length);
            }
            finally
            {
                fs?.Close();
            }
        }

        public void AddFlie(string name, string filename, byte[] fileData, int dataLength)
        {
            if (dataLength <= 0 || dataLength > fileData.Length)
            {
                dataLength = fileData.Length;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", _boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n", name, filename);
            sb.AppendFormat("Content-Type: {0}\r\n", GetContentType(filename));
            sb.Append("\r\n");
            byte[] buf = _encoding.GetBytes(sb.ToString());
            _ms.Write(buf, 0, buf.Length);
            _ms.Write(fileData, 0, dataLength);
            byte[] crlf = _encoding.GetBytes("\r\n");
            _ms.Write(crlf, 0, crlf.Length);
        }

        public void AddString(string name, string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", _boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n", name);
            sb.Append("\r\n");
            sb.AppendFormat("{0}\r\n", value);
            byte[] buf = _encoding.GetBytes(sb.ToString());
            _ms.Write(buf, 0, buf.Length);
        }

        private string GetContentType(string filename)
        {
            Microsoft.Win32.RegistryKey fileExtKey = null; ;
            string contentType = "application/stream";
            try
            {
                fileExtKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename ?? ""));

                if (fileExtKey != null)
                    contentType = fileExtKey.GetValue("Content Type", contentType).ToString();
            }
            finally
            {
                fileExtKey?.Close();
            }
            return contentType;
        }
    }

}