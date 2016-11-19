using System;

namespace Mammatus.Library.TimeZone
{
    public class DateTimeHelper
    {
        /// <summary>
        /// Parse the date using the Invariant (system) culture.
        /// 
        /// Since you're specifying a mask, the culture should be irrelevant.
        /// 
        /// Throws an exception if the parse isn't exact.
        /// </summary>
        /// <param name="s">Date string to parse</param>
        /// <param name="mask">Mask to use</param>
        /// <returns>DateTime parsed</returns>
        public static DateTime ParseExact(string s, string mask)
        {
            return DateTime.ParseExact(s, mask, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
        }

        /// <summary>
        /// Returns the time between 2 dates formatted as h:mm:ss
        /// </summary>
        public static string ElapsedHMS(DateTime start, DateTime end)
        {
            var t = end - start;
            // Weirdo format required http://stackoverflow.com/questions/12543349/timespan-tostring-d-hhmm
            return t.ToString("h':'mm':'ss");
        }

        /// <summary>
        /// Returns the time between 2 dates formatted as s.x (1 decimal place of partial seconds)
        /// </summary>
        public static string ElapsedSm(DateTime start, DateTime end)
        {
            var t = end - start;
            return t.TotalSeconds.ToString("0.0");
        }
    }
}
