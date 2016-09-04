using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mammatus.String.Extensions
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

        private static Regex regex = new Regex(@"[^0-9]");

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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
            if (int.TryParse(regex.Replace(text, ""), out numi))
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
    }
}
