using System;
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

    }
}
