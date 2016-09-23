using System;
using Mammatus.Time;

namespace Mammatus.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTimeDiff Days(this int offset)
        {
            return new DateTimeDiff(offset, (x, y) => x.AddDays(y));
        }

        public static DateTimeDiff Month(this int offset)
        {
            return new DateTimeDiff(offset, (x, y) => x.AddMonths(y));
        }

        public static DateTimeDiff Years(this int offset)
        {
            return new DateTimeDiff(offset, (x, y) => x.AddYears(y));
        }

         Timestamps

        /// <summary>
        /// Converts a timestamp to a DateTime
        /// </summary>
        /// <param name="timestamp">The timestamp (milliseconds unix epoch)</param>
        /// <returns>The date time</returns>
        public static DateTime ToDateTime(this long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// Converts a DateTime to a timestamp.
        /// </summary>
        /// <param name="datetime">The original date time</param>
        /// <returns>The timestamp (milliseconds unix epoch)</returns>
        public static long ToTimestamp(this DateTime datetime)
        {
            TimeSpan ts = (datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (long)ts.TotalMilliseconds;
        }

        

         Comparing to Now

        /// <summary>
        /// Elapseds time from now.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>The elapsed timespan from now.</returns>
        public static TimeSpan Elapsed(this DateTime datetime)
        {
            return DateTime.Now - datetime;
        }

        /// <summary>
        /// Indicates whether the date is older than now.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>True if the date is older than now.</returns>
        public static bool HasPassed(this DateTime datetime)
        {
            return datetime.Elapsed() > TimeSpan.Zero;
        }

        
    }
}
