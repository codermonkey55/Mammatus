using Mammatus.Time;
using System;

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

        /// <summary>
        /// Returns first day of week for given DateTime instance
        /// </summary>
        /// <param name="dateTimeValue">DateTime instance</param>
        /// <returns>Result as DateTime instance</returns>
        public static DateTime FirstDayOfWeek(this DateTime dateTimeValue)
        {
            switch (dateTimeValue.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return dateTimeValue;
                case DayOfWeek.Tuesday:
                    return dateTimeValue.AddDays(-1);
                case DayOfWeek.Wednesday:
                    return dateTimeValue.AddDays(-2);
                case DayOfWeek.Thursday:
                    return dateTimeValue.AddDays(-3);
                case DayOfWeek.Friday:
                    return dateTimeValue.AddDays(-4);
                case DayOfWeek.Saturday:
                    return dateTimeValue.AddDays(-5);
                case DayOfWeek.Sunday:
                    return dateTimeValue.AddDays(-6);
                default:
                    {
                        return dateTimeValue;
                    }
            }
        }

        /// <summary>
        /// Returns first day of month for given DateTime instance
        /// </summary>
        /// <param name="dateTimeValue">DateTime instance</param>
        /// <returns>Result as DateTime instance</returns>
        public static DateTime FirstDayOfMonth(this DateTime dateTimeValue)
        {
            return dateTimeValue.AddDays(-(dateTimeValue.Day - 1));
        }

        /// <summary>
        /// Returns last day of month for given DateTime instance
        /// </summary>
        /// <param name="dateTimeValue">DateTime instance</param>
        /// <returns>Result as DateTime instance</returns>
        public static DateTime LastDayOfMonth(this DateTime dateTimeValue)
        {
            return dateTimeValue.AddMonths(1).AddDays(-dateTimeValue.Day);
        }

        /// <summary>
        /// Converts the given UTC <see cref="DateTime"/> to the specified <see cref="TimeZoneInfo"/> using the system local TimeZoneId.
        /// </summary>
        /// <param name="utcDateTimeValue"></param>
        /// <param name="timeZoneid"></param>
        /// <returns></returns>
        public static DateTime ToTimeZone(this DateTime utcDateTimeValue, string timeZoneid)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTimeValue, TimeZoneInfo.FindSystemTimeZoneById(timeZoneid));
        }

        #region Timestamps

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

        #endregion

        #region Comparing to Now

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

        #endregion

    }
}
