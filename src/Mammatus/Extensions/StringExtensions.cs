using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Mammatus.Extensions
{
    public static class StringExtensions
    {
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
        public static string RemoveDuplicateWhitespace(this string input)
        {
            return Regex.Replace(input, @"\s", " ", RegexOptions.Compiled);
        }
        public static string RemoveWhitespace(this string input)
        {
            return Regex.Replace(input, @"\s", "", RegexOptions.Compiled);
        }
        public static bool IsIn(this string input, params string[] parameters)
        {
            return input.IsIn(StringComparison.Ordinal, parameters);
        }
        public static bool IsIn(this string input, StringComparison comparison, params string[] parameters)
        {
            if (parameters == null)
                return false;

            return parameters.Any(d => string.Equals(input, d, comparison));
        }
        public static TEnum ToEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }
        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
        public static TEnum ParseEnumByName<TEnum>(this string enumNameString) where TEnum : struct
        {
            TEnum outputEnum;

            if (string.IsNullOrEmpty(enumNameString))
            {
                outputEnum = default(TEnum);

                return outputEnum;
            }

            var textInfo = new CultureInfo("en-US", true).TextInfo;

            var enumNameStringTitleCase = textInfo.ToTitleCase(enumNameString);

            Enum.TryParse<TEnum>(enumNameStringTitleCase.Trim(), out outputEnum);

            return outputEnum;
        }
        public static bool TryParseEnumByName<TEnum>(this string enumNameString, out TEnum outputEnum) where TEnum : struct
        {
            bool parseSuccesful = false;

            if (string.IsNullOrEmpty(enumNameString))
            {
                outputEnum = default(TEnum);

                return parseSuccesful;
            }

            var textInfo = new CultureInfo("en-US", true).TextInfo;

            var enumNameStringTitleCase = textInfo.ToTitleCase(enumNameString);

            parseSuccesful = Enum.TryParse<TEnum>(enumNameStringTitleCase.Trim(), out outputEnum);

            return parseSuccesful;
        }



        public static T ConvertStringToEnum<T>(string value) where T : struct
        {
            return ConvertStringToEnum<T>(value, true);
        }
        public static T ConvertStringToEnum<T>(string value, bool ignoreCase) where T : struct
        {
            T retValue = default(T);
            if (!string.IsNullOrEmpty(value) && Enum.TryParse<T>(value, ignoreCase, out retValue))
                return retValue;

            return default(T);
        }



        public static bool PartialMatch(this IReadOnlyList<string> origin, IReadOnlyList<string> reference)
        {
            if (origin == null || reference == null || origin.Count < reference.Count)
            {
                return false;
            }

            for (int i = 0; i < reference.Count; i++)
            {
                if (reference[i] == null || origin[i].Equals(reference[i]))
                {
                    continue;
                }
                return false;
            }
            return true;
        }
        public static string Join(this IEnumerable<string> list, string separator)
        {
            if (list == null)
            {
                return null;
            }
            return string.Join(separator, list);
        }
        public static string Join(this IEnumerable<string> list)
        {
            if (list == null)
            {
                return null;
            }
            return string.Join("", list);
        }
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }
        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
        public static bool HasText(this string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
        private static readonly Regex _regex = new Regex(@"[^0-9]");
        public static double ToDouble(string text)
        {
            //double
            double num;
            if (double.TryParse(text, out num))
            {
                return num;
            }

            //16
            if (text.StartsWith("0x"))
            {
                try
                {
                    var i = Convert.ToInt32(text, 16);
                    return i;
                }
                catch
                {
                    //no operation
                }
            }

            //
            int numi;
            if (int.TryParse(_regex.Replace(text, ""), out numi))
            {
                return numi;
            }

            return 0.0;
        }
        /// <summary>
        ///
        /// http://dobon.net/vb/dotnet/string/detectcode.html
        /// </summary>
        /// <remarks>
        /// Jcode.
        /// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
        /// Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
        /// </remarks>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static System.Text.Encoding GetCode(this byte[] bytes)
        {
            const byte bEscape = 0x1B;
            const byte bAt = 0x40;
            const byte bDollar = 0x24;
            const byte bAnd = 0x26;
            const byte bOpen = 0x28;    //'('
            const byte bB = 0x42;
            const byte bD = 0x44;
            const byte bJ = 0x4A;
            const byte bI = 0x49;

            int len = bytes.Length;
            byte b1, b2, b3, b4;

            //Encode::is_utf8

            bool isBinary = false;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 <= 0x06 || b1 == 0x7F || b1 == 0xFF)
                {
                    //'binary'
                    isBinary = true;
                    if (b1 == 0x00 && i < len - 1 && bytes[i + 1] <= 0x7F)
                    {
                        //smells like raw unicode
                        return System.Text.Encoding.Unicode;
                    }
                }
            }
            if (isBinary)
            {
                return null;
            }

            //not Japanese
            bool notJapanese = true;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 == bEscape || 0x80 <= b1)
                {
                    notJapanese = false;
                    break;
                }
            }
            if (notJapanese)
            {
                return System.Text.Encoding.ASCII;
            }

            for (int i = 0; i < len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                b3 = bytes[i + 2];

                if (b1 == bEscape)
                {
                    if (b2 == bDollar && b3 == bAt)
                    {
                        //JIS_0208 1978
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bDollar && b3 == bB)
                    {
                        //JIS_0208 1983
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && (b3 == bB || b3 == bJ))
                    {
                        //JIS_ASC
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && b3 == bI)
                    {
                        //JIS_KANA
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    if (i < len - 3)
                    {
                        b4 = bytes[i + 3];
                        if (b2 == bDollar && b3 == bOpen && b4 == bD)
                        {
                            //JIS_0212
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                        if (i < len - 5 &&
                            b2 == bAnd && b3 == bAt && b4 == bEscape &&
                            bytes[i + 4] == bDollar && bytes[i + 5] == bB)
                        {
                            //JIS_0208 1990
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                    }
                }
            }

            //should be euc|sjis|utf8
            //use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
            int sjis = 0;
            int euc = 0;
            int utf8 = 0;
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
                    ((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC)))
                {
                    //SJIS_C
                    sjis += 2;
                    i++;
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
                    (b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF)))
                {
                    //EUC_C
                    //EUC_KANA
                    euc += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
                        (0xA1 <= b3 && b3 <= 0xFE))
                    {
                        //EUC_0212
                        euc += 3;
                        i += 2;
                    }
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF))
                {
                    //UTF8
                    utf8 += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
                        (0x80 <= b3 && b3 <= 0xBF))
                    {
                        //UTF8
                        utf8 += 3;
                        i += 2;
                    }
                }
            }
            //M. Takahashi's suggestion
            //utf8 += utf8 / 2;

            //System.Diagnostics.Debug.WriteLine(
            //    string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));

            if (euc > sjis && euc > utf8)
            {
                //EUC
                return System.Text.Encoding.GetEncoding(51932);
            }
            else if (sjis > euc && sjis > utf8)
            {
                //SJIS
                return System.Text.Encoding.GetEncoding(932);
            }
            else if (utf8 > euc && utf8 > sjis)
            {
                //UTF8
                return System.Text.Encoding.UTF8;
            }

            return null;
        }



        /// <summary>
        /// The regular expression pattern for validating an email.
        /// </summary>
        public const string EmailPattern = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)";
        /// <summary>
        /// Indicates whether a string is a valid email.
        /// </summary>
        /// <param name="text">The email string that must be validated.</param>
        /// <returns>Returns true if the given string is an email, else false.</returns>
        public static bool IsValidEmail(this string text)
        {
            if (String.IsNullOrEmpty(text))
                return false;

            text = text.Trim();

            try
            {
                return Regex.IsMatch(text, @"\A" + EmailPattern + @"\Z", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        /// <summary>
        /// Extracts all email from a given string.
        /// </summary>
        /// <param name="text">The given string.</param>
        /// <returns>A collection of all email found in the given string.</returns>
        public static IEnumerable<String> ExtractEmails(this string text)
        {
            Regex rx = new Regex(EmailPattern, RegexOptions.IgnoreCase);
            return rx.Matches(text).Cast<Match>().Select((m) => m.Value.ToString());
        }
        /// <summary>
        /// Default MIME type for a resource.
        /// </summary>
        private const string DefaultMimeType = "application/octet-stream";
        /// <summary>
        /// Mapping from file extensions to corresponding MIME types.
        /// </summary>
        private static readonly Dictionary<string, string> FileExtensionsMimes = new Dictionary<string, string>()
        {
            // Text
            { "css", "text/css"},
            { "csv", "text/csv"},
            { "html", "text/html"},
            { "htm", "text/html"},
            { "txt", "text/plain"},

            // Image

            { "jpg", "image/jpeg"},
            { "jpeg", "image/jpeg"},
            { "bmp", "image/bmp"},
            { "png", "image/png"},
            { "gif", "image/gif"},
            { "tiff", "image/tiff"},
            { "tif", "image/tiff"},

            // Audio

            { "mp3", "audio/mpeg"},
            { "wav", "audio/x-wav"},
            { "wma", "audio/x-ms-wma"},

            // Video

            { "mp4", "video/mp4" },
            { "wmv", "video/x-ms-wmv"},

            // Application

            { "pdf", "application/pdf"},
            { "json", "application/json"},
            { "xml", "application/xml"},
            { "zip", "application/zip"},
            { "ogg", "application/ogg"},
            { "js", "application/javascript"},

            // Office

            { "xls", "application/vnd.ms-excel"},
            { "ppt", "application/vnd.ms-powerpoint"},
            { "doc", "application/msword"},
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        };
        /// <summary>
        /// Returns the MIME type associated to a file path (based on its extension).
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>The MIME type if found, else a default one.</returns>
        public static string GetMimeType(this string path)
        {
            var ext = Path.GetExtension(path);

            if (!String.IsNullOrEmpty(ext))
            {
                ext = ext.TrimStart('.');

                if (FileExtensionsMimes.ContainsKey(ext))
                {
                    return FileExtensionsMimes[ext];
                }
            }

            return DefaultMimeType;
        }



        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        public static string[] GetStrArray(string str)
        {
            return str.Split(new Char[] { ',' });
        }
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in list)
            {
                sb.Append(kvp.Value + ",");
            }
            if (list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
        }
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }
        public static string ToSBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "ÇëÊäÈëÐèÒª»®·Ö¸ñÊ½µÄ×Ö·û´®";
            }
            else
            {
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "ÑùÊ½¸ñÊ½µÄ³¤¶ÈÓëÊäÈëµÄ×Ö·û³¤¶È²»·û£¬ÇëÖØÐÂÊäÈë";
                }
                else
                {
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    ReturnValue = StrList;
                    Error = "";
                }
            }
            return ReturnValue;
        }
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if ((str != null) && (str != ""))
            {
                strArray = new Regex(splitstr).Split(str);
            }
            return strArray;
        }
        public static string SqlSafeString(string String, bool IsDel)
        {
            if (IsDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }
            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }



        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }
        public static string GetMD5(string s)
        {
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();

            return s.ToLower().Substring(8, 16);
        }
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");
                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");
                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");
                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");
                case "4":
                    return dateTime1.ToString("MM-dd");
                case "5":
                    return dateTime1.ToString("MM/dd");
                case "6":
                    return dateTime1.ToString("MM月dd日");
                case "7":
                    return dateTime1.ToString("yyyy-MM");
                case "8":
                    return dateTime1.ToString("yyyy/MM");
                case "9":
                    return dateTime1.ToString("yyyy年MM月");
                default:
                    return dateTime1.ToString();
            }
        }
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            @"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);",
            @"&(nbsp|#160);",
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }
        public static bool IsNullOrEmpty<T>(T data)
        {
            if (data == null)
            {
                return true;
            }

            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            return false;
        }
        public static bool IsNullOrEmpty(object data)
        {
            if (data == null)
            {
                return true;
            }

            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            return false;
        }
        public static bool IsIP(string ip)
        {
            if (IsNullOrEmpty(ip))
            {
                return true;
            }

            //
            ip = ip.Trim();

            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            return Regex.IsMatch(ip, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsEmail(string email)
        {
            if (IsNullOrEmpty(email))
            {
                return false;
            }

            //
            email = email.Trim();

            string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsInt(string number)
        {
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            number = number.Trim();

            string pattern = @"^[0-9]+[0-9]*$";

            return Regex.IsMatch(number, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsNumber(string number)
        {
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            number = number.Trim();

            string pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";

            return Regex.IsMatch(number, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsDate(ref string date)
        {
            if (IsNullOrEmpty(date))
            {
                return true;
            }

            date = date.Trim();

            date = date.Replace(@"\", "-");

            date = date.Replace(@"/", "-");

            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                if (!IsInt(date))
                {
                    return false;
                }

                if (date.Length == 8)
                {
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                if (date.Length == 6)
                {
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                if (date.Length == 5)
                {
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    date = year + "-" + month;
                    return true;
                }

                if (date.Length == 4)
                {
                    string year = date.Substring(0, 4);

                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }


                return false;
            }
        }
        public static bool IsIdCard(string idCard)
        {
            if (IsNullOrEmpty(idCard))
            {
                return true;
            }

            idCard = idCard.Trim();

            StringBuilder pattern = new StringBuilder();
            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");

            return Regex.IsMatch(idCard, pattern.ToString(), RegexOptions.IgnoreCase);
        }
        public static bool IsValidInput(ref string input)
        {
            try
            {
                if (IsNullOrEmpty(input))
                {
                    return true;
                }
                else
                {
                    input = input.Replace("'", "''").Trim();

                    string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                    string[] testArray = testString.Split('|');
                    foreach (string testStr in testArray)
                    {
                        if (input.ToLower().IndexOf(testStr) != -1)
                        {
                            input = "";
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public static string GetQueryString(string name)
        {
            return HttpContext.Current.Request.QueryString[name] ?? "";
        }
        public static string GetFormEntry(string name)
        {
            return HttpContext.Current.Request.Form[name] ?? "";
        }
        public static string GetHtml(string sDetail)
        {
            Match match;

            sDetail = sDetail.Replace(" ", "&nbsp;");
            sDetail = sDetail.Replace("'", "’");
            sDetail = sDetail.Replace("\"", "&quot;");
            sDetail = sDetail.Replace("<", "&lt;");
            sDetail = sDetail.Replace(">", "&gt;");

            var regex = new Regex(@"(\r\n((&nbsp;)|　)+)(?<正文>\S+)", RegexOptions.IgnoreCase);

            for (match = regex.Match(sDetail); match.Success; match = match.NextMatch())
            {
                sDetail = sDetail.Replace(match.Groups[0].ToString(), "<BR>　　" + match.Groups["正文"]);
            }

            sDetail = sDetail.Replace("\r\n", "<BR>");

            return sDetail;
        }



        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from);
                string result = Convert.ToString(intValue, to);
                if (to == 2)
                {
                    int resultLength = result.Length;
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {
                return "0";
            }
        }
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }
    }
}