using Mammatus.Helpers;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace Mammatus.Utils
{
    public static class Tools
    {
        public static string GetUserIp()
        {
            string ip;
            string[] temp;
            var isErr = false;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"];
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (var i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }

            if (isErr)
                return "1.1.1.1";
            return ip;
        }

        public static string GetMD5(string s)
        {
            s = FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5");

            return s.ToLower().Substring(8, 16);
        }

        public static int StrLength(string inputString)
        {
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var s = ascii.GetBytes(inputString);
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }

        public static string ClipString(string inputString, int len)
        {
            var isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var tempString = "";
            var s = ascii.GetBytes(inputString);
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            var mybyte = Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "…";
            return tempString;
        }

        public static TimeSpan DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            var ts1 = new TimeSpan(DateTime1.Ticks);
            var ts2 = new TimeSpan(DateTime2.Ticks);
            var ts = ts1.Subtract(ts2).Duration();
            return ts;
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
            var random = new Random();
            var minTime = new DateTime();
            var maxTime = new DateTime();

            var ts = new TimeSpan(time1.Ticks - time2.Ticks);

            var dTotalSecontds = ts.TotalSeconds;
            var iTotalSecontds = 0;

            if (dTotalSecontds > int.MaxValue)
            {
                iTotalSecontds = int.MaxValue;
            }
            else if (dTotalSecontds < int.MinValue)
            {
                iTotalSecontds = int.MinValue;
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

            var maxValue = iTotalSecontds;

            if (iTotalSecontds <= int.MinValue)
                maxValue = int.MinValue + 1;

            var i = random.Next(Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }

        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg =
            {
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

            var newReg = aryReg[0];
            var strOutput = strHtml;
            for (var i = 0; i < aryReg.Length; i++)
            {
                var regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
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

            if (data.GetType() == typeof(string))
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

            if (data.GetType() == typeof(string))
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

            ip = ip.Trim();

            var pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            return RegexHelper.IsMatch(ip, pattern);
        }

        public static bool IsEmail(string email)
        {
            if (IsNullOrEmpty(email))
            {
                return false;
            }

            email = email.Trim();

            var pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            return RegexHelper.IsMatch(email, pattern);
        }

        public static bool IsInt(string number)
        {
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            number = number.Trim();

            var pattern = @"^[0-9]+[0-9]*$";

            return RegexHelper.IsMatch(number, pattern);
        }

        public static bool IsNumber(string number)
        {
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            number = number.Trim();

            var pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";

            return RegexHelper.IsMatch(number, pattern);
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

            if (date.IndexOf("") != -1)
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
                    var year = date.Substring(0, 4);
                    var month = date.Substring(4, 2);
                    var day = date.Substring(6, 2);

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
                    var year = date.Substring(0, 4);
                    var month = date.Substring(4, 2);

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
                    var year = date.Substring(0, 4);
                    var month = date.Substring(4, 1);

                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    date = year + "-" + month;
                    return true;
                }

                if (date.Length == 4)
                {
                    var year = date.Substring(0, 4);

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

            var pattern = new StringBuilder();
            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");

            return RegexHelper.IsMatch(idCard, pattern.ToString());
        }

        public static bool IsValidInput(ref string input)
        {
            try
            {
                if (IsNullOrEmpty(input))
                {
                    return true;
                }
                input = input.Replace("'", "''").Trim();

                var testString =
                    "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                var testArray = testString.Split('|');
                foreach (var testStr in testArray)
                {
                    if (input.ToLower().IndexOf(testStr) != -1)
                    {
                        input = "";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}