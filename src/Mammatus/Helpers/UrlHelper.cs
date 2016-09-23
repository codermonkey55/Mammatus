using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Mammatus.Helpers
{
    using System;

    public class UrlHelper
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static string Base64Encrypt(string sourceUrl)
        {
            string eurl = HttpUtility.UrlEncode(sourceUrl);
            eurl = Convert.ToBase64String(Encoding.GetBytes(eurl));
            return eurl;
        }

        public static string Base64Decrypt(string encryptedStr)
        {
            if (!IsBase64(encryptedStr))
            {
                return encryptedStr;
            }
            byte[] buffer = Convert.FromBase64String(encryptedStr);
            string sourthUrl = Encoding.GetString(buffer);
            sourthUrl = HttpUtility.UrlDecode(sourthUrl);
            return sourthUrl;
        }

        public static bool IsBase64(string eStr)
        {
            if ((eStr.Length % 4) != 0)
            {
                return false;
            }
            if (!Regex.IsMatch(eStr, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static string AddParam(string url, string paramName, string value)
        {
            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query))
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "?" + paramName + "=" + eval);
            }
            else
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "&" + paramName + "=" + eval);
            }
        }

        public static string UpdateParam(string url, string paramName, string value)
        {
            string keyWord = paramName + "=";
            int index = url.IndexOf(keyWord) + keyWord.Length;
            int index1 = url.IndexOf("&", index);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, value);
                return url;
            }
            url = url.Remove(index, index1 - index);
            url = url.Insert(index, value);
            return url;
        }

        public static void GetDomain(string fromUrl, out string domain, out string subDomain)
        {
            domain = "";
            subDomain = "";
            try
            {
                if (fromUrl.IndexOf("µÄÃûÆ¬") > -1)
                {
                    subDomain = fromUrl;
                    domain = "ÃûÆ¬";
                    return;
                }

                UriBuilder builder = new UriBuilder(fromUrl);
                fromUrl = builder.ToString();

                Uri u = new Uri(fromUrl);

                if (u.IsWellFormedOriginalString())
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "¿Í»§¶Ë±¾µØÎÄ¼þÂ·¾¶";

                    }
                    else
                    {
                        string Authority = u.Authority;
                        string[] ss = u.Authority.Split('.');
                        if (ss.Length == 2)
                        {
                            Authority = "www." + Authority;
                        }
                        int index = Authority.IndexOf('.', 0);
                        domain = Authority.Substring(index + 1, Authority.Length - index - 1).Replace("comhttp", "com");
                        subDomain = Authority.Replace("comhttp", "com");
                        if (ss.Length < 2)
                        {
                            domain = "²»Ã÷Â·¾¶";
                            subDomain = "²»Ã÷Â·¾¶";
                        }
                    }
                }
                else
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "¿Í»§¶Ë±¾µØÎÄ¼þÂ·¾¶";
                    }
                    else
                    {
                        subDomain = domain = "²»Ã÷Â·¾¶";
                    }
                }
            }
            catch
            {
                subDomain = domain = "²»Ã÷Â·¾¶";
            }
        }

        public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            nvc = new NameValueCollection();
            baseUrl = "";

            if (url == "")
                return;

            int questionMarkIndex = url.IndexOf('?');

            if (questionMarkIndex == -1)
            {
                baseUrl = url;
                return;
            }
            baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return;
            string ps = url.Substring(questionMarkIndex + 1);

            // ¿ªÊ¼·ÖÎö²ÎÊý¶Ô
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);

            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
        }
    }
}
