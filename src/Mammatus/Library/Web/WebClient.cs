using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Mammatus.Library.Web
{
    using System;
    using System.IO;

    public class WebClient
    {
        private Encoding _encoding = Encoding.Default;
        private string _respHtml = "";
        private WebProxy _proxy;
        private static CookieContainer _cc;
        private readonly WebHeaderCollection _requestHeaders;
        private WebHeaderCollection _responseHeaders;
        private int _bufferSize = 15240;
        public event EventHandler<UploadEventArgs> UploadProgressChanged;
        public event EventHandler<DownloadEventArgs> DownloadProgressChanged;

        static WebClient()
        {
            LoadCookiesFromDisk();
        }

        public WebClient()
        {
            _requestHeaders = new WebHeaderCollection();
            _responseHeaders = new WebHeaderCollection();
        }

        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        public WebHeaderCollection ResponseHeaders
        {
            get { return _responseHeaders; }
        }

        public WebHeaderCollection RequestHeaders
        {
            get { return _requestHeaders; }
        }

        public WebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        public string RespHtml
        {
            get { return _respHtml; }
            set { _respHtml = value; }
        }

        public CookieContainer CookieContainer
        {
            get { return _cc; }
            set { _cc = value; }
        }

        public string GetHtml(string url)
        {
            HttpWebRequest request = CreateRequest(url, "GET");
            _respHtml = _encoding.GetString(GetData(request));
            return _respHtml;
        }

        public void DownloadFile(string url, string filename)
        {
            FileStream fs = null;
            try
            {
                HttpWebRequest request = CreateRequest(url, "GET");
                byte[] data = GetData(request);
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                fs.Write(data, 0, data.Length);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        public byte[] GetData(string url)
        {
            HttpWebRequest request = CreateRequest(url, "GET");
            return GetData(request);
        }

        public string Post(string url, string postData)
        {
            byte[] data = _encoding.GetBytes(postData);
            return Post(url, data);
        }

        public string Post(string url, byte[] postData)
        {
            HttpWebRequest request = CreateRequest(url, "POST");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.KeepAlive = true;
            PostData(request, postData);
            _respHtml = _encoding.GetString(GetData(request));
            return _respHtml;
        }

        public string Post(string url, MultipartForm mulitpartForm)
        {
            HttpWebRequest request = CreateRequest(url, "POST");
            request.ContentType = mulitpartForm.ContentType;
            request.ContentLength = mulitpartForm.FormData.Length;
            request.KeepAlive = true;
            PostData(request, mulitpartForm.FormData);
            _respHtml = _encoding.GetString(GetData(request));
            return _respHtml;
        }

        private byte[] GetData(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            _responseHeaders = response.Headers;
            //SaveCookiesToDisk();

            DownloadEventArgs args = new DownloadEventArgs();
            if (_responseHeaders[HttpResponseHeader.ContentLength] != null)
                args.TotalBytes = Convert.ToInt32(_responseHeaders[HttpResponseHeader.ContentLength]);

            MemoryStream ms = new MemoryStream();
            int count = 0;
            byte[] buf = new byte[_bufferSize];
            while ((count = stream.Read(buf, 0, buf.Length)) > 0)
            {
                ms.Write(buf, 0, count);
                if (DownloadProgressChanged != null)
                {
                    args.BytesReceived += count;
                    args.ReceivedData = new byte[count];
                    Array.Copy(buf, args.ReceivedData, count);
                    DownloadProgressChanged(this, args);
                }
            }
            stream.Close();
            //½âÑ¹
            if (ResponseHeaders[HttpResponseHeader.ContentEncoding] != null)
            {
                MemoryStream msTemp = new MemoryStream();
                count = 0;
                buf = new byte[100];
                switch (ResponseHeaders[HttpResponseHeader.ContentEncoding].ToLower())
                {
                    case "gzip":
                        GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress);
                        while ((count = gzip.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();
                    case "deflate":
                        DeflateStream deflate = new DeflateStream(ms, CompressionMode.Decompress);
                        while ((count = deflate.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();
                    default:
                        break;
                }
            }
            return ms.ToArray();
        }

        private void PostData(HttpWebRequest request, byte[] postData)
        {
            int offset = 0;
            int sendBufferSize = _bufferSize;
            int remainBytes = 0;
            Stream stream = request.GetRequestStream();
            UploadEventArgs args = new UploadEventArgs();
            args.TotalBytes = postData.Length;
            while ((remainBytes = postData.Length - offset) > 0)
            {
                if (sendBufferSize > remainBytes) sendBufferSize = remainBytes;
                stream.Write(postData, offset, sendBufferSize);
                offset += sendBufferSize;
                if (UploadProgressChanged != null)
                {
                    args.BytesSent = offset;
                    UploadProgressChanged(this, args);
                }
            }
            stream.Close();
        }

        private HttpWebRequest CreateRequest(string url, string method)
        {
            Uri uri = new Uri(url);

            if (uri.Scheme == "https")
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            // Set a default policy level for the "http:" and "https" schemes.
            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
            HttpWebRequest.DefaultCachePolicy = policy;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = false;
            request.AllowWriteStreamBuffering = false;
            request.Method = method;
            if (_proxy != null)
                request.Proxy = _proxy;
            request.CookieContainer = _cc;
            foreach (string key in _requestHeaders.Keys)
            {
                request.Headers.Add(key, _requestHeaders[key]);
            }
            _requestHeaders.Clear();
            return request;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        private static void SaveCookiesToDisk()
        {
            string cookieFile = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Create);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formater.Serialize(fs, _cc);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        private static void LoadCookiesFromDisk()
        {
            _cc = new CookieContainer();
            string cookieFile = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
            if (!File.Exists(cookieFile))
                return;
            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                _cc = (CookieContainer)formater.Deserialize(fs);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
    }

}