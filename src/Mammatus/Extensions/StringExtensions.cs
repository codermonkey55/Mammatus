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
            return IsMatch("^[1-9]*[0-9]*$", _value);
        }
        public static bool IsMatch(this string _value, string regexExpression)
        {
            if (_value == null) return false;
            if (_value.Length == 0) return false;
            Regex myRegex = new Regex(regexExpression);
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



        public static string Interpolate(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }
        public static string Fmt(this string formatString, params object[] args)
        {
            return Interpolate(formatString, args);
        }
        public static string Last(this string value, int count)
        {
            if (count > value.Length) throw new ArgumentOutOfRangeException(string.Format("Cannot return more characters than exist in the string (wanted {0} string contains {1}", count, value.Length));

            return value.Substring(value.Length - count, count);
        }
        public static string SnakeCase(this string camelizedString)
        {
            var parts = new List<string>();
            var currentWord = new StringBuilder();

            foreach (var c in camelizedString)
            {
                if (char.IsUpper(c) && currentWord.Length > 0)
                {
                    parts.Add(currentWord.ToString());
                    currentWord = new StringBuilder();
                }
                currentWord.Append(char.ToLower(c));
            }

            if (currentWord.Length > 0)
            {
                parts.Add(currentWord.ToString());
            }

            return string.Join("_", parts.ToArray());
        }
        public static string Capitalize(this string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1);
        }




        public static bool EqualsIgnoreCase(this string text, string toCheck)
        {
            return string.Equals(text, toCheck, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsToAnyIgnoreCase(this string text, params string[] toCheck)
        {
            if (toCheck == null)
                return false;

            foreach (var t in toCheck)
            {
                if (string.Equals(text, t, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        //public static bool EqualsIgnoreCase(this char c, char toCheck)
        //{
        //    var cup = char.ToUpper(c);
        //    var toCheckUp = char.ToUpper(toCheck);
        //    return cup.Equals(toCheckUp);
        //}

        //public static bool EqualsToAnyIgnoreCase(this char c, params char[] toCheck)
        //{
        //    var clow = char.ToUpper(c);

        //    for (int i = 0; i < toCheck.Length; i++)
        //    {
        //        if (clow == char.ToUpper(toCheck[i]))
        //            return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Indica si el string contine el texto toCheck lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toCheck">Texto a busar</param>
        /// <returns>True if the string contains the text toCheck ignoring case.</returns>
        public static bool ContainsIgnoreCase(this string text, string toCheck)
        {
            if (toCheck.IsNullOrEmpty())
                throw new ArgumentException("El parametro 'toCheck' es vacio");

            return text.IndexOfIgnoreCase(toCheck) >= 0;
        }

        public static int IndexWholePhrase(this string text, string toCheck, int startIndex = 0)
        {
            if (toCheck.IsNullOrEmpty())
                throw new ArgumentException("El parametro 'toCheck' es vacio");

            //var startIndex = 0;
            while (startIndex <= text.Length)
            {
                var index = text.IndexOfIgnoreCase(startIndex, toCheck);
                if (index < 0)
                    return -1;

                var indexPreviousCar = index - 1;
                var indexNextCar = index + toCheck.Length;
                if ((index == 0
                     || !Char.IsLetter(text[indexPreviousCar]))
                    && (index + toCheck.Length == text.Length
                        || !Char.IsLetter(text[indexNextCar])))
                    return index;

                startIndex = index + toCheck.Length;
            }
            return -1;
        }


        /// <summary>
        /// Take all the words in the input string and separate them.
        /// </summary>
        ///
        ///

        private static readonly Regex mSplitWords = new Regex(@"\W+");

        public static string[] SplitInWords(this string s)
        {

            //
            // Split on all non-word characters.
            // ... Returns an array of all the words.
            //
            return mSplitWords.Split(s);
            // @      special verbatim string syntax
            // \W+    one or more non-word characters together
        }

        public static List<string> SplitInWordsLongerThan(this string s, int wordlength)
        {
            var res = new List<string>();
            var splitwords = mSplitWords.Split(s);

            foreach (var word in splitwords)
            {
                if (word.Length > wordlength)
                {
                    res.Add(word);
                }
            }

            return res;
        }

        public static int TotalWords(this string text)
        {
            text = text.Trim();

            if (text.IsNullOrEmpty())
                return 0;

            var res = 0;

            var prevCharIsSeparator = true;
            foreach (var c in text)
            {
                if (char.IsSeparator(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c))
                {
                    if (!prevCharIsSeparator)
                        res++;
                    prevCharIsSeparator = true;
                }
                else
                    prevCharIsSeparator = false;
            }
            if (!prevCharIsSeparator)
                res++;

            return res;
        }

        public static IEnumerable<string> EnumerateLines(this string s)
        {
            var index = 0;

            while (true)
            {
                var newIndex = s.IndexOf(Environment.NewLine, index, StringComparison.Ordinal);
                if (newIndex < 0)
                {
                    if (s.Length > index)
                        yield return s.Substring(index);

                    yield break;
                }

                var currentString = s.Substring(index, newIndex - index);
                index = newIndex + 2;

                yield return currentString;
            }
        }


        public static int CountLines(this string s)
        {
            var index = 0;
            var lines = 0;

            while (true)
            {
                var newIndex = s.IndexOf(Environment.NewLine, index, StringComparison.Ordinal);
                if (newIndex < 0)
                {
                    if (s.Length > index)
                        lines++;

                    return lines;
                }

                index = newIndex + 2;
                lines++;
            }
        }

        public static string[] SplitInLines(this string s)
        {
            return s.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public static T[] SplitInLinesTyped<T>(this string s) where T : IComparable
        {
            return SplitTyped<T>(s, Environment.NewLine);
        }

        public static string[] SplitInLinesRemoveEmptys(this string s)
        {
            return s.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }


        public static Tuple<string, string> SplitByIndex(this string text, int index)
        {
            if (text.IsNullOrEmpty())
                return new Tuple<string, string>("", "");

            if (index >= text.Length)
                return new Tuple<string, string>(text, "");

            if (index <= 0)
                return new Tuple<string, string>("", text);

            return new Tuple<string, string>(text.Substring(0, index - 1), text.Substring(index - 1));
        }

        public static Tuple<string, string> SplitWordsByIndex(this string text, int index)
        {
            var splitByIndex = text.SplitByIndex(index);
            var res = new Tuple<string, string>(splitByIndex.Item1, splitByIndex.Item2);

            var wordsInItem1 = res.Item1.SplitInWords();
            res = new Tuple<string, string>(wordsInItem1.Take(wordsInItem1.Length - 1).JoinToString(" ").Trim(), wordsInItem1.Last() + splitByIndex.Item2);

            return res;
        }

        public static bool ContainsWholeWord(this string text, string toCheck)
        {
            if (text.IsNullOrEmpty())
                return false;

            if (toCheck.IsNullOrEmpty())
                throw new ArgumentException("El parametro 'toChek' es vacio");

            var partes = text.SplitInWords();
            foreach (var parte in partes)
            {
                if (parte.EqualsIgnoreCase(toCheck))
                    return true;
            }
            return false;
        }

        public static bool ContainsAnyWholeWord(this string text, params string[] toCheck)
        {
            if (text.IsNullOrEmpty())
                return false;

            if (toCheck == null || toCheck.Length == 0)
                throw new ArgumentException("El parametro 'toChek' es vacio");

            var partes = text.SplitInWords();
            foreach (var parte in partes)
            {
                foreach (var check in toCheck)
                {
                    if (parte.EqualsIgnoreCase(check))
                        return true;
                }
            }
            return false;
        }

        public static bool ContainsWholePhrase(this string text, string toCheck)
        {
            if (toCheck.IsNullOrEmpty())
                throw new ArgumentException("El parametro 'toChek' es vacio");

            var startIndex = 0;
            while (startIndex <= text.Length)
            {
                var index = text.IndexOfIgnoreCase(startIndex, toCheck);
                if (index < 0)
                    return false;

                var indexPreviousCar = index - 1;
                var indexNextCar = index + toCheck.Length;
                if ((index == 0
                     || !Char.IsLetter(text[indexPreviousCar]))
                    && (index + toCheck.Length == text.Length
                        || !Char.IsLetter(text[indexNextCar])))
                    return true;

                startIndex = index + toCheck.Length;
            }
            return false;
        }

        public static bool ContainsWholePhraseAny(this string text, params string[] toCheck)
        {
            foreach (var phrase in toCheck)
            {
                if (text.ContainsWholePhrase(phrase)) return true;
            }
            return false;
        }

        public static bool ContainsWholePhraseAll(this string text, params string[] toCheck)
        {
            foreach (var phrase in toCheck)
            {
                if (!text.ContainsWholePhrase(phrase)) return false;
            }
            return true;
        }

        public static string FindFirstPhrase(this string text, params string[] phrasesToCheck)
        {
            if (phrasesToCheck == null || phrasesToCheck.Length == 0)
                throw new ArgumentException("El parametro 'phrasesToCheck' es vacio");

            return phrasesToCheck.FirstOrDefault(checking => text.ContainsWholePhrase(checking));
        }

        public static bool IsSameWords(this string text, string check)
        {
            if (check.IsNullOrEmpty())
                return text.IsNullOrEmpty();

            var wordsText = text.SplitInWords();
            var wordsCheck = check.SplitInWords();

            var wordsTextNotInCheck = wordsText.FindAll(x => !x.IsOn(wordsCheck));
            if (wordsTextNotInCheck.Length > 0)
                return false;

            var wordsCheckNotInText = wordsCheck.FindAll(x => !x.IsOn(wordsText));
            if (wordsCheckNotInText.Length > 0)
                return false;

            return true;
        }


        /// <summary>
        /// Indica si el string contine algunos de los textos toCheck lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toCheck">Texto a busar</param>
        /// <returns>True if the string contains the text toCheck ignoring case.</returns>
        public static bool ContainsAnyIgnoreCase(this string text, params string[] toCheck)
        {
            if (toCheck == null || toCheck.Length == 0)
                throw new ArgumentException("El parametro 'toChek' es vacio");

            foreach (var checking in toCheck)
            {
                if (text.IndexOfIgnoreCase(checking) >= 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Indica si el string contine todos los textos toCheck, lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toCheck">Texto a buscar</param>
        /// <returns>True if the string contains all the texts toCheck ignoring case.</returns>
        public static bool ContainsAllIgnoreCase(this string text, params string[] toCheck)
        {
            if (toCheck == null || toCheck.Length == 0)
                throw new ArgumentException("El parametro 'toChek' es vacio");

            foreach (var checking in toCheck)
            {
                if (text.IndexOfIgnoreCase(checking) < 0) return false;
            }
            return true;
        }

        public static bool ContainsOnlyThisChar(this string text, char toCheck)
        {
            if (text.Length == 0)
                return false;

            foreach (var t in text)
            {
                if (t != toCheck) return false;
            }
            return true;
        }



        /// <summary>
        /// Indica en que lugar el string contine el texto toCheck lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toCheck">Texto a busar</param>
        /// <returns>El indice donde aparece por primera vez la cadena toCheck ignorando el case.</returns>
        public static int IndexOfIgnoreCase(this string text, string toCheck)
        {
            return text.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Indica el ultimo indixe donde el string contine el texto toCheck lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toCheck">Texto a busar</param>
        /// <returns>El indice donde aparece por primera vez la cadena toCheck ignorando el case.</returns>
        public static int LastIndexOfIgnoreCase(this string text, string toCheck)
        {
            return text.LastIndexOf(toCheck, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Indica el ultimo indixe donde el string contine el texto toCheck lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toCheck">Texto a busar</param>
        /// <returns>El indice donde aparece por primera vez la cadena toCheck ignorando el case.</returns>
        public static int LastIndexOfIgnoreCase(this string text, string toCheck, int startIndex, int count)
        {
            return text.LastIndexOf(toCheck, startIndex, count, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Indica en que lugar el string contine el texto toCheck lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startIndex">La posicion inicial de la busqueda</param>
        /// <param name="toCheck">Texto a busar</param>
        /// <returns>El indice donde aparece por primera vez la cadena toCheck ignorando el case.</returns>
        public static int IndexOfIgnoreCase(this string text, int startIndex, string toCheck)
        {
            return text.IndexOf(toCheck, startIndex, StringComparison.OrdinalIgnoreCase);
        }

        public static string FindFirstOcurrence(this string text, params string[] toCheck)
        {
            if (toCheck == null || toCheck.Length == 0)
                throw new ArgumentException("El parametro 'toCheck' es vacio");

            return toCheck.FirstOrDefault(checking => text.ContainsIgnoreCase(checking));
        }

        public static int LastIndexOfAny(this string text, params string[] toCheck)
        {
            if (toCheck == null || toCheck.Length == 0)
                throw new ArgumentException("El parametro 'toCheck' es vacio");

            var res = -1;
            foreach (var checking in toCheck)
            {
                var index = text.LastIndexOf(checking, StringComparison.OrdinalIgnoreCase);
                if (index >= 0
                    && index > res)
                    res = index;
            }
            return res;
        }


        public static string FindAndReplaceFirstOcurrence(this string text, string newValue, params string[] oldValues)
        {
            if (oldValues == null || oldValues.Length == 0)
                throw new ArgumentException("El parametro 'oldValues' es vacio");

            foreach (var oldValue in oldValues)
            {
                if (text.ContainsIgnoreCase(oldValue))
                    return text.ReplaceIgnoringCase(oldValue, newValue);
            }
            return text;
        }

        public static string FindAndInsertBeforeFirstOcurrence(this string text, string textInsert, params string[] oldValues)
        {
            if (oldValues == null || oldValues.Length == 0)
                throw new ArgumentException("El parametro 'oldValues' es vacio");

            foreach (var oldValue in oldValues)
            {
                if (text.ContainsIgnoreCase(oldValue))
                    return text.ReplaceIgnoringCase(oldValue, textInsert + oldValue);
            }
            return text;
        }

        /// <summary>
        /// Indica si el string comienza con el texto toCheck lo busca con OrdinalIgnoringCase
        /// </summary>
        /// <param name="text"></param>
        /// <param name="toCheck">Texto a busar</param>
        public static bool StartsWithIgnoreCase(this string text, string toCheck)
        {
            if (toCheck.IsNullOrEmpty())
                return true;

            if (text.IsNullOrEmpty())
                return toCheck.IsNullOrEmpty();

            return text.StartsWith(toCheck, StringComparison.OrdinalIgnoreCase);
        }

        public static bool StartsWithAnyIgnoreCase(this string text, params string[] toCheck)
        {
            return StartsWithAnyIgnoreCase(text, (IEnumerable<string>)toCheck);
        }

        public static bool StartsWithAnyIgnoreCase(this string text, IEnumerable<string> toCheck)
        {
            if (text.IsNullOrEmpty())
                return false;

            foreach (var check in toCheck)
            {
                if (text.StartsWith(check, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        public static bool EndsWithIgnoreCase(this string text, string toCheck)
        {
            if (toCheck.IsNullOrEmpty())
                return true;

            if (text.IsNullOrEmpty())
                return toCheck.IsNullOrEmpty();

            return text.EndsWith(toCheck, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EndsWithAnyIgnoreCase(this string text, params string[] toCheck)
        {
            return EndsWithAnyIgnoreCase(text, (IEnumerable<string>)toCheck);
        }

        public static bool EndsWithAnyIgnoreCase(this string text, IEnumerable<string> toCheck)
        {
            if (text.IsNullOrEmpty())
                return false;

            foreach (var check in toCheck)
            {
                if (text.EndsWith(check, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        //private static readonly CultureInfo CultureHelper.CultureUSA = new CultureInfo("en-us");
        //private static CultureInfo CultureHelper.Argentina = new CultureInfo("es-ar");


        public static string ToLink(this string text, string href, string target = "")
        {
            return "<a href='" + href + "' " + (target.IsNullOrEmpty() ? "" : " target='" + target + "'") + ">" + text +
                   "</a>";
        }


        /// <summary>
        /// Remueve la cadena especificada si se encuentra
        /// </summary>
        /// <param name="text"></param>
        /// <param name="removeText">Texto a ser removido</param>
        /// <returns>Texto sin la cadena a ser removida</returns>
        public static string Remove(this string text, string removeText)
        {
            return text.Replace(removeText, String.Empty);
        }

        public static string RemoveFromIgnoreCase(this string text, string removeFromThis)
        {
            int index = text.IndexOfIgnoreCase(removeFromThis);

            if (index < 0)
                return text;
            else
                return text.Substring(0, index);
        }

        /// <summary>
        /// Determina si una cadena NO es Nula o Vacia
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>Verdadero o Falso</returns>
        public static bool NotIsNullOrEmpty(this string texto)
        {
            return !String.IsNullOrEmpty(texto);
        }


        /// <summary>
        /// Determina si una cadena es Nula, vacia o solo tiene caracteres blancos (tab, space, enter)
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>Verdadero o Falso</returns>
        public static bool IsNullOrWhite(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return true;

            texto = texto.Trim();
            return string.IsNullOrEmpty(texto);
        }

        /// <summary>
        /// Determina si una cadena NO es Nula, vacia o solo tiene caracteres blancos (tab, space, enter)
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>Verdadero o Falso</returns>
        public static bool NotIsNullOrWhite(this string texto)
        {
            return !texto.IsNullOrWhite();
        }

        /// <summary>
        /// Determina si una cadena es Vacia
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>Verdadero o Falso</returns>
        public static bool IsEmpty(this string texto)
        {
            return texto.Equals(String.Empty);
        }

        public static string Substring(this string text, string startText)
        {
            int indice = text.IndexOf(startText);
            if (indice == -1) throw new ArgumentException("No se encontro: " + startText);
            return text.Substring(indice);
        }

        public static string Right(this string text, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length > 0", "length");
            }
            if ((length == 0) || (text == null))
            {
                return "";
            }
            int strLength = text.Length;
            if (length >= strLength)
            {
                return text;
            }
            return text.Substring(strLength - length, length);
        }

        public static string Left(this string text, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException("Length > 0", "length");
            }
            if ((length == 0) || (text == null))
            {
                return "";
            }
            if (length >= text.Length)
            {
                return text;
            }
            return text.Substring(0, length);
        }

        /// <summary>
        ///  Inidica si alguno de los valores esta en la cadena
        /// </summary>
        /// <param name="text"></param>
        /// <param name="values">Valores a verificar</param>
        /// <returns>Verdadero o Falso</returns>
        public static bool Contains(this string text, string[] values)
        {
            bool contain = false;
            foreach (String s in values)
            {
                if (text.Contains(s)) contain = true;
            }
            return contain;
        }

        /// <summary>
        ///  Le hace un Trim a todas las cadenas de la lista
        /// </summary>
        /// <param name="textos"></param>
        /// <returns></returns>
        public static void TrimAll(this IList<string> textos)
        {
            for (int i = 0; i < textos.Count; i++)
            {
                textos[i] = textos[i].Trim();
            }
        }
        /// <summary>
        /// Es como llamar a String.Format pero direcmante sobre el string  "Hola {0} hoy es: {1}".Fill("Juan", 12)
        /// </summary>
        /// <param name="original"></param>
        /// <param name="values">Los mismos valores que se les manda al String.Format</param>
        /// <returns></returns>
        public static string Fill(this string original, params object[] values)
        {
            string texto =
                original.Replace("\\n", Environment.NewLine)
                        .Replace("<br>", Environment.NewLine)
                        .Replace("<BR>", Environment.NewLine);

            return string.Format(texto, values);
        }

        public static string ReplaceIgnoringCase(this string original, string oldValue, string newValue)
        {
            return Replace(original, oldValue, newValue, StringComparison.OrdinalIgnoreCase);
        }

        public static string ReplaceOnlyWholePhrase(this string original, string oldValue, string newValue)
        {
            if (original.IsNullOrEmpty()
                || oldValue.IsNullOrEmpty())
                return original;

            var index = original.IndexWholePhrase(oldValue);
            var lastIndex = 0;

            var buffer = new StringBuilder(original.Length);

            while (index >= 0)
            {
                buffer.Append(original, startIndex: lastIndex, count: index - lastIndex);
                buffer.Append(newValue);

                lastIndex = index + oldValue.Length;

                index = original.IndexWholePhrase(oldValue, startIndex: index + 1);
            }
            buffer.Append(original, lastIndex, original.Length - lastIndex);

            return buffer.ToString();
        }

        public static string ReplaceFirstOccurrence(this string original, string oldValue, string newValue)
        {
            if (oldValue.IsNullOrEmpty())
                return original;

            var index = original.IndexOfIgnoreCase(oldValue);

            if (index < 0)
                return original;

            else if (index == 0)
                return newValue + original.Substring(oldValue.Length);
            else
                return original.Substring(0, index) + newValue + original.Substring(index + oldValue.Length);
        }

        public static string ReplaceLastOccurrence(this string original, string oldValue, string newValue)
        {
            if (oldValue.IsNullOrEmpty())
                return original;

            var index = original.LastIndexOfIgnoreCase(oldValue);

            if (index < 0)
                return original;
            else if (index == 0)
                return newValue + original.Substring(oldValue.Length);
            else
                return original.Substring(0, index) + newValue + original.Substring(index + oldValue.Length);
        }

        public static string ReplaceOnlyAtEndIgnoreCase(this string original, string oldValue, string newValue)
        {
            if (oldValue.IsNullOrEmpty())
                return original;

            if (original.EndsWithIgnoreCase(oldValue))
                return original.Substring(0, original.Length - oldValue.Length) + newValue;

            return original;
        }


        public static string Replace(this string original, string oldValue, string newValue, StringComparison comparisionType)
        {
            if (original.IsNullOrEmpty())
                return original;

            string result = original;

            if (!string.IsNullOrEmpty(oldValue))
            {
                int index = -1;
                int lastIndex = 0;

                var buffer = new StringBuilder(original.Length);

                while ((index = original.IndexOf(oldValue, index + 1, comparisionType)) >= 0)
                {
                    buffer.Append(original, lastIndex, index - lastIndex);
                    buffer.Append(newValue);

                    lastIndex = index + oldValue.Length;
                }
                buffer.Append(original, lastIndex, original.Length - lastIndex);

                result = buffer.ToString();
            }

            return result;
        }

        public static string Truncate(this string original, int maxLength)
        {
            if (original.IsNullOrEmpty() || maxLength == 0)
                return string.Empty;
            if (original.Length <= maxLength)
                return original;
            else if (maxLength <= 3)
                return original.Substring(0, 2) + ".";
            else
                return original.Substring(0, maxLength - 3) + "...";
        }

        public static string ReplaceRecursive(this string original, string oldValue, string newValue)
        {
            const int MaxTries = 1000;

            string ante, res;

            res = original.Replace(oldValue, newValue);

            var i = 0;
            do
            {
                i++;
                ante = res;
                res = ante.Replace(oldValue, newValue);

            } while (ante != res || i > MaxTries);

            return res;
        }

        public static string ToValidIdentifier(this string original)
        {
            original = original.ToCapitalCase();

            if (original.Length == 0)
                return "_";

            StringBuilder res = new StringBuilder(original.Length + 1);
            if (!char.IsLetter(original[0]) && original[0] != '_')
                res.Append('_');

            for (int i = 0; i < original.Length; i++)
            {
                char c = original[i];
                if (char.IsLetterOrDigit(c) || c == '_')
                    res.Append(c);
                else
                    res.Append('_');
            }

            return res.ToString().ReplaceRecursive("__", "_").Trim('_');
        }


        public static string ToCapitalCase(this string original)
        {
            var words = original.Split(' ');
            var result = new List<string>();
            foreach (var word in words)
            {
                if (word.Length == 0 || AllCapitals(word))
                    result.Add(word);
                else if (word.Length == 1)
                    result.Add(word.ToUpper());
                else
                    result.Add(Char.ToUpper(word[0]) + word.Remove(0, 1).ToLower());
            }

            return string.Join(" ", result).Replace(" Y ", " y ")
                .Replace(" De ", " de ")
                .Replace(" O ", " o ");

        }

        static bool AllCapitals(string input)
        {
            return input.ToCharArray().All(Char.IsUpper);
        }

        public static string ToCamelCase(this string original)
        {
            if (original.Length <= 1)
                return original.ToLower();

            return char.ToLower(original[0]) + original.Substring(1);
        }

        public static string UseAsSeparatorFor<T>(this string separator, IEnumerable<T> list)
        {
            return list.JoinToString(separator);
        }
        public static string AvoidNull(this string original)
        {
            if (original == null)
                return string.Empty;

            return original;
        }


        /// <summary>
        /// Repite un string la cantidad de veces indicada
        /// </summary>
        /// <param name="text"></param>
        /// <param name="times">Texto a busar</param>
        /// <returns>The <paramref name="text"/> repited the number of times indicates in the parameters</returns>
        public static string Repeat(this string text, int times)
        {
            if (text.IsNullOrEmpty() || times == 0)
                return string.Empty;

            if (text.Length == 1)
                return new string(text[0], times);

            if (times == 1)
                return text;
            else if (times == 2)
                return string.Concat(text, text);
            else if (times == 3)
                return string.Concat(text, text, text);
            else if (times == 4)
                return string.Concat(text, text, text, text);

            StringBuilder res = new StringBuilder(text.Length * times);
            for (int i = 0; i < times; i++)
            {
                res.Append(text);
            }
            return res.ToString();
        }

        /// <summary>
        /// Extrae el texto alrededor de un indice de un string
        /// </summary>
        /// <param name="text"></param>
        public static string ExtractAround(this string text, int index, int left, int right)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            if (index >= text.Length)
                throw new IndexOutOfRangeException("The parameter index is outside the limits of the string.");

            var startIndex = Math.Max(0, index - left);
            var length = Math.Min(text.Length - startIndex, index - startIndex + right);

            return text.Substring(startIndex, length);
        }


        public static string TrimPhrase(this string text, string phrase)
        {
            var res = TrimPhraseStart(text, phrase);
            res = TrimPhraseEnd(res, phrase);
            return res;
        }

        public static string TrimPhraseStart(this string text, string phrase)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            if (phrase.IsNullOrEmpty())
                return text;

            while (text.StartsWith(phrase))
            {
                text = text.Substring(phrase.Length);
            }

            return text;
        }

        public static string TrimPhraseEnd(this string text, string phrase)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            if (phrase.IsNullOrEmpty())
                return text;

            while (text.EndsWithIgnoreCase(phrase))
            {
                text = text.Substring(0, text.Length - phrase.Length);
            }

            return text;
        }


        //public static string TrimPhraseEndIgnoreCase(this string text, string phrase)
        //{
        //    if (text.IsNullOrEmpty())
        //        return string.Empty;

        //    if (phrase.IsNullOrEmpty())
        //        return text;

        //    while (text.EndsWithIgnoreCase(phrase))
        //    {
        //        text = text.Substring(0, text.Length - phrase.Length);
        //    }

        //    return text;
        //}

        public static bool TryParseNumber(this string original)
        {
            long res;

            if (long.TryParse(original, out res))
                return true;
            else
                return false;
        }


        public static bool Like(this string me, string pattern)
        {
            //Turn a SQL-like-pattern into regex, by turning '%' into '.*'
            //Doesn't handle SQL's underscore into single character wild card '.{1,1}',
            // or the way SQL uses square brackets for escaping.
            //(Note the same concept could work for DOS-style wildcards (* and ?)
            var regex = new Regex("^" + pattern
                           .Replace(".", "\\.")
                           .Replace("*", ".*")
                           .Replace("%", ".*")
                           .Replace("\\.*", "\\%")
                           + "$", RegexOptions.IgnoreCase);

            return regex.IsMatch(me);
        }


        public static bool MatchRegEx(this string me, string pattern)
        {
            //Turn a SQL-like-pattern into regex, by turning '%' into '.*'
            //Doesn't handle SQL's underscore into single character wild card '.{1,1}',
            // or the way SQL uses square brackets for escaping.
            //(Note the same concept could work for DOS-style wildcards (* and ?)
            return Regex.IsMatch(me, pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }


        public static string RemoveDuplicateSpaces(this string me)
        {
            if (me.IsNullOrEmpty())
                return me;

            string ante = null;
            while (ante != me)
            {
                ante = me;
                me = me.Replace("  ", " ");
            }
            return me;
        }

        public static string RemoveDuplicateChar(this string me, char charRemove)
        {
            if (me.IsNullOrEmpty())
                return me;

            var strChar = charRemove.ToString();
            var charRep = strChar + strChar;

            string ante = null;
            while (ante != me)
            {
                ante = me;
                me = me.Replace(charRep, strChar);
            }
            return me;
        }

        public static T[] SplitTyped<T>(this string me, char delimiter)
            where T : IComparable
        {
            if (me.IsNullOrWhite())
                return new T[] { };

            me = me.Trim();

            var parts = me.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            var res = new T[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                res[i] = (T)Convert.ChangeType(parts[i], typeof(T));
            }
            return res;
        }

        public static T[] SplitTyped<T>(this string me, string delimiter)
         where T : IComparable
        {
            if (me.IsNullOrWhite())
                return new T[] { };

            me = me.Trim();

            var parts = me.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            var res = new T[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                res[i] = (T)Convert.ChangeType(parts[i], typeof(T));
            }
            return res;
        }
        public static string LastWord(this string me)
        {
            if (me.IsNullOrEmpty())
                return string.Empty;

            for (int i = me.Length - 1; i >= 0; i--)
            {
                if (char.IsSeparator(me, i))
                {
                    if (i == me.Length - 1) // Si el separador es el ultimo entonces vacio
                        return string.Empty;
                    else
                        return me.Substring(i + 1);
                }

            }
            return me;
        }
        public static string SecondWord(this string me)
        {
            if (me.IsNullOrEmpty())
                return string.Empty;

            var parts = me.SplitInWords();
            if (parts.Length >= 2)
                return parts[1];
            else
                return string.Empty;
        }


        public static string FirstWord(this string me)
        {
            if (me.IsNullOrEmpty())
                return string.Empty;

            for (int i = 0; i < me.Length; i++)
            {
                if (char.IsSeparator(me, i))
                {
                    if (i == 0) // Si el separador es el ultimo entonces vacio
                        return string.Empty;
                    else
                        return me.Substring(0, i);
                }

            }
            return me;
        }

        public static string RemoveChars(this string me, params char[] toRemove)
        {
            var res = new StringBuilder(me);
            foreach (var remove in toRemove)
            {
                res.Replace(remove, char.MinValue);
            }
            res.Replace(char.MinValue.ToString(), string.Empty);
            return res.ToString();
        }

        public static string ReplaceCharsWithSpace(this string me, params char[] toReplace)
        {
            var res = new StringBuilder(me);
            foreach (var replace in toReplace)
            {
                res.Replace(replace, ' ');
            }
            return res.ToString();
        }

        public static string ReplaceNumbersWith(this string me, char toReplace)
        {
            var res = new StringBuilder(me.Length);
            foreach (var caracter in me)
            {
                if (caracter.IsOn('1', '2', '3', '4', '5', '6', '7', '8', '9', '0'))
                    res.Append(toReplace);
                else
                    res.Append(caracter);
            }
            return res.ToString();
        }

        public static string SubstringFrom(this string me, string from)
        {
            if (me.IsNullOrEmpty())
                return string.Empty;

            var index = me.IndexOfIgnoreCase(from);
            if (index < 0)
                return string.Empty;

            return me.Substring(index + from.Length);
        }

        public static string SubstringTo(this string me, string from)
        {
            if (me.IsNullOrEmpty())
                return string.Empty;

            var index = me.IndexOfIgnoreCase(from);
            if (index < 0)
                return string.Empty;

            return me.Substring(0, index);
        }

        #region Contains digits and letters revisar redundancias


        public static string OnlyLettersNumbers(this string text)
        {
            var res = new StringBuilder(text.Length);

            foreach (char car in text)
            {
                if (char.IsLetterOrDigit(car))
                    res.Append(car);
            }

            return res.ToString();
        }
        public static string FilterChars(this string text, Predicate<char> onlyThese)
        {
            var res = new StringBuilder(text.Length);

            foreach (char car in text)
            {
                if (onlyThese(car))
                    res.Append(car);
            }

            return res.ToString();
        }


        public static bool IsUpper(this string text)
        {
            foreach (var ch in text)
            {
                if (!char.IsLetter(ch)
                    || char.IsLower(ch))
                    return false;
            }

            return true;
        }

        public static bool IsLower(this string text)
        {
            foreach (var ch in text)
            {
                if (char.IsLetter(ch)
                    && char.IsUpper(ch))
                    return false;
            }

            return true;
        }

        public static bool ContainsOnlyDigits(this string text) //ver
        {
            foreach (var car in text)
            {
                if (!char.IsDigit(car)) return false;
            }
            return true;
        }

        public static string OnlyDigits(this string text) //ver
        {
            return text.OnlyDigits(null);
        }

        public static bool NotContainsDigits(this string text) //Ver
        {
            foreach (var car in text)
            {
                if (char.IsDigit(car)) return false;
            }
            return true;
        }

        public static bool ContainsDigit(this string text) //ver
        {
            foreach (var car in text)
            {
                if (char.IsDigit(car)) return true;
            }
            return false;
        }

        public static string OnlyDigits(this string text, IEnumerable<char> excepciones) //ver
        {
            var res = new StringBuilder();
            foreach (char car in text)
            {
                if (char.IsDigit(car)
                    || excepciones.Contains(car))
                    res.Append(car);
            }

            return res.ToString();
        }

        public static bool IncludeDigits(this string text) //ver
        {
            return text.IncludeDigits(0);
        }

        public static bool IncludeDigits(this string text, int minCount) //ver
        {
            var count = 0;
            foreach (var car in text)
            {
                if (char.IsDigit(car))
                    count++;

                if (count >= minCount)
                    return true;
            }

            return false;
        }

        public static bool IncludeLetters(this string text) //ver
        {
            return text.IncludeLetters(0);
        }

        public static bool IncludeLetters(this string text, int minCount) //ver
        {
            var count = 0;
            foreach (var car in text)
            {
                if (char.IsLetter(car))
                    count++;

                if (count >= minCount)
                    return true;
            }

            return false;
        }

        public static int TotalDigits(this string text)
        {
            if (text.IsNullOrEmpty())
                return 0;

            return text.ToCharArray().FindAll(x => char.IsDigit(x)).Length;
        }

        public static int TotalLetters(this string text)
        {
            if (text.IsNullOrEmpty())
                return 0;

            return text.ToCharArray().FindAll(x => char.IsLetter(x)).Length;
        }

        public static int TotalLowerLetters(this string text)
        {
            if (text.IsNullOrEmpty())
                return 0;

            return text.ToCharArray().FindAll(x => char.IsLetter(x) && char.IsLower(x)).Length;
        }

        public static int TotalUpperLetters(this string text)
        {
            if (text.IsNullOrEmpty())
                return 0;

            return text.ToCharArray().FindAll(x => char.IsLetter(x) && char.IsUpper(x)).Length;
        }

        #endregion

        public static int CountOccurrences(this string text, char toCheck)
        {
            return text.CountOccurrences(toCheck.ToString());
        }

        public static int CountOccurrences(this string text, string toCheck)
        {
            if (toCheck.IsNullOrEmpty())
                return 0;

            int res = 0;
            int posIni = 0;
            while ((posIni = text.IndexOfIgnoreCase(posIni, toCheck)) != -1)
            {
                posIni += toCheck.Length;
                res++;
            }

            return res;
        }

        public static bool DiffOnlyOneChar(this string text, string toCheck)
        {
            if (text.Length != toCheck.Length)
                throw new ArgumentException("Los parametros para DiffOnlyOneChar deben tener la misma longitud");

            return text.DiffCharsCount(toCheck) == 1;
        }

        public static int DiffCharsCount(this string text, string toCheck)
        {
            if (text.Length != toCheck.Length)
                throw new ArgumentException("Los parametros para DiffOnlyOneChar deben tener la misma longitud");

            int res = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != toCheck[i])
                    res++;
            }
            return res;
        }

        public static int DiffCharsCountIgnoreCase(this string text, string toCheck)
        {
            if (text.Length != toCheck.Length)
                throw new ArgumentException("Los parametros para DiffOnlyOneChar deben tener la misma longitud");

            int res = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (!text[i].EqualsIgnoreCase(toCheck[i]))
                    res++;
            }
            return res;
        }

        public static bool OneAbsentChar(this string text, string toCheck)
        {
            if (text.Length > 1
                && toCheck.Length > 1
                && Math.Abs(text.Length - toCheck.Length) != 1) //las long deben diferir en 1, y ambas ser mayor que 1
                return false;

            var textWithChar = (text.Length > toCheck.Length ? text : toCheck);
            var textNoChar = (text.Length > toCheck.Length ? toCheck : text);

            //chequear si es el ultimo
            if (textWithChar[textWithChar.Length - 1] != textNoChar[textNoChar.Length - 1])
                return textWithChar.Substring(0, textWithChar.Length - 1).EqualsIgnoreCase(textNoChar);

            for (int i = 0; i < textNoChar.Length; i++)
            {
                if (textWithChar[i] != textNoChar[i])
                {
                    //a partir del car distinto, el resto debe coincidir
                    return textWithChar.Substring(i + 1).EqualsIgnoreCase(textNoChar.Substring(i));
                }
            }
            return false;
        }


        public static string SafeGroupValue(this Match match, string name)
        {
            var group = match.Groups[name];

            if (group == null)
                return null;

            return match.Groups[name].Value;
        }



        #region "  Char Extensions  "

        public static bool EqualsIgnoreCase(this char text, char toCheck)
        {
            return char.ToUpper(text) == char.ToUpper(toCheck);
        }

        public static bool EqualsIgnoreCase(this char? text, char toCheck)
        {
            if (text == null)
                return false;

            return char.ToUpper(text.Value) == char.ToUpper(toCheck);
        }

        /// <summary>
        /// Return the AscII value of a char
        /// </summary>
        /// <param name="c"></param>
        /// <returns>AscII value of the char</returns>
        public static int ASCIIValue(this char c)
        {
            int num;
            int num2 = Convert.ToInt32(c);
            if (num2 < 0x80)
            {
                return num2;
            }

            byte[] buffer;
            Encoding fileIOEncoding = Encoding.UTF8;

            char[] chars = new char[] { c };
            if (fileIOEncoding.GetMaxByteCount(1) == 1)
            {
                buffer = new byte[1];
                int num3 = fileIOEncoding.GetBytes(chars, 0, 1, buffer, 0);
                return buffer[0];
            }
            buffer = new byte[2];
            if (fileIOEncoding.GetBytes(chars, 0, 1, buffer, 0) == 1)
            {
                return buffer[0];
            }
            if (BitConverter.IsLittleEndian)
            {
                byte num4 = buffer[0];
                buffer[0] = buffer[1];
                buffer[1] = num4;
            }
            num = BitConverter.ToInt16(buffer, 0);

            return num;
        }

        #endregion

    }
}