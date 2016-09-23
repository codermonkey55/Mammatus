using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Mammatus.Helpers
{
    using System;
    using System.IO;

    public class HtmlHelper
    {
        private static string _contentType = "application/x-www-form-urlencoded";
        private static string _accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static string _userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        private static Encoding _encoding = Encoding.GetEncoding("utf-8");
        private static int _delay = 1000;
        private static int _maxTry = 300;
        private static int _currentTry = 0;

        public static CookieContainer CookieContainer { get; } = new CookieContainer();

        public static Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;
            }
        }

        public static int NetworkDelay
        {
            get
            {
                Random r = new Random();
                return (r.Next(_delay, _delay * 2));
            }
            set
            {
                _delay = value;
            }
        }

        public static int MaxTry
        {
            get
            {
                return _maxTry;
            }
            set
            {
                _maxTry = value;
            }
        }

        public static string GetHtml(string url, string postData, bool isPost, CookieContainer cookieContainer)
        {
            if (string.IsNullOrEmpty(postData)) return GetHtml(url, cookieContainer);
            Thread.Sleep(NetworkDelay);
            _currentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                byte[] byteRequest = Encoding.Default.GetBytes(postData);
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = _contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = _maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = _accept;
                httpWebRequest.UserAgent = _userAgent;
                httpWebRequest.Method = isPost ? "POST" : "GET";
                httpWebRequest.ContentLength = byteRequest.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(byteRequest, 0, byteRequest.Length);
                stream.Close();
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, _encoding);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                _currentTry = 0;
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch (Exception e)
            {
                if (_currentTry <= _maxTry) GetHtml(url, postData, isPost, cookieContainer);
                _currentTry--;
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                return string.Empty;
            }
        }

        public static string GetHtml(string url, CookieContainer cookieContainer)
        {
            Thread.Sleep(NetworkDelay);
            _currentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = _contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = _maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = _accept;
                httpWebRequest.UserAgent = _userAgent;
                httpWebRequest.Method = "GET";
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, _encoding);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                _currentTry--;
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch (Exception e)
            {
                if (_currentTry <= _maxTry) GetHtml(url, cookieContainer);
                _currentTry--;
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                return string.Empty;
            }
        }

        //---------------------------------------------------------------------------------------------------------------
        // System.Net.CookieContainer cookie = new System.Net.CookieContainer();
        // Stream s = HttpHelper.GetStream("http://ptlogin2.qq.com/getimage?aid=15000102&0.43878429697395826", cookie);
        // picVerify.Image = Image.FromStream(s);
        //---------------------------------------------------------------------------------------------------------------

        public static Stream GetStream(string url, CookieContainer cookieContainer)
        {
            _currentTry++;

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = _contentType;
                httpWebRequest.ServicePoint.ConnectionLimit = _maxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = _accept;
                httpWebRequest.UserAgent = _userAgent;
                httpWebRequest.Method = "GET";

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                _currentTry--;
                return responseStream;
            }
            catch (Exception e)
            {
                if (_currentTry <= _maxTry)
                {
                    GetHtml(url, cookieContainer);
                }

                _currentTry--;

                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                }
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return null;
            }
        }

        public static string NoHtml(string htmlstring)
        {
            //删除脚本
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            htmlstring = regex.Replace(htmlstring, "");
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            htmlstring.Replace("<", "");
            htmlstring.Replace(">", "");
            htmlstring.Replace("\r\n", "");

            return htmlstring;
        }

        public string GetHref(string htmlCode)
        {
            string matchVale = "";
            string reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)[\S]*";
            foreach (Match m in Regex.Matches(htmlCode, reg))
            {
                matchVale += (m.Value).ToLower().Replace("href=", "").Trim() + "|";
            }
            return matchVale;
        }

        public string GetImgSrc(string htmlCode, string imgHttp)
        {
            string matchVale = "";
            string reg = @"<img.+?>";
            foreach (Match m in Regex.Matches(htmlCode.ToLower(), reg))
            {
                matchVale += GetImg((m.Value).ToLower().Trim(), imgHttp) + "|";
            }

            return matchVale;
        }

        public string GetImg(string imgString, string imgHttp)
        {
            string matchVale = "";
            string reg = @"src=.+\.(bmp|jpg|gif|png|)";
            foreach (Match m in Regex.Matches(imgString.ToLower(), reg))
            {
                matchVale += (m.Value).ToLower().Trim().Replace("src=", "");
            }
            if (matchVale.IndexOf(".net") != -1 || matchVale.IndexOf(".com") != -1 || matchVale.IndexOf(".org") != -1 || matchVale.IndexOf(".cn") != -1 || matchVale.IndexOf(".cc") != -1 || matchVale.IndexOf(".info") != -1 || matchVale.IndexOf(".biz") != -1 || matchVale.IndexOf(".tv") != -1)
                return (matchVale);
            else
                return (imgHttp + matchVale);
        }

        public static string Get_Http(string tUrl)
        {
            string strResult;
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(tUrl);
                hwr.Timeout = 19600;
                HttpWebResponse hwrs = (HttpWebResponse)hwr.GetResponse();
                Stream myStream = hwrs.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder sb = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    sb.Append(sr.ReadLine() + "\r\n");
                }
                strResult = sb.ToString();
                hwrs.Close();
            }
            catch (Exception ee)
            {
                strResult = ee.Message;
            }
            return strResult;
        }

        public static string Post_Http(string url, string postData, string encodeType)
        {
            string strResult = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodeType);
                byte[] post = encoding.GetBytes(postData);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = post.Length;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(post, 0, post.Length); //设置POST
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                strResult = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }

        public static string ZipHtml(string html)
        {
            html = Regex.Replace(html, @">\s+?<", "><");//去除HTML中的空白字符
            html = Regex.Replace(html, @"\r\n\s*", "");
            html = Regex.Replace(html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", @"<body$1>$2</body>", RegexOptions.IgnoreCase);
            return html;
        }

        public static string DelHtml(string sTextStr, string htmlStr)
        {
            string rStr = "";
            if (!string.IsNullOrEmpty(sTextStr))
            {
                rStr = Regex.Replace(sTextStr, "<" + htmlStr + "[^>]*>", "", RegexOptions.IgnoreCase);
                rStr = Regex.Replace(rStr, "</" + htmlStr + ">", "", RegexOptions.IgnoreCase);
            }
            return rStr;
        }

        public static string File(string path, System.Web.UI.Page p)
        {
            return @p.ResolveUrl(path);
        }

        public static string Css(string cssPath, System.Web.UI.Page p)
        {
            return @"<link href=""" + p.ResolveUrl(cssPath) + @""" rel=""stylesheet"" type=""text/css"" />" + "\r\n";
        }

        public static string Js(string jsPath, System.Web.UI.Page p)
        {
            return @"<script type=""text/javascript"" src=""" + p.ResolveUrl(jsPath) + @"""></script>" + "\r\n";
        }

    }
}