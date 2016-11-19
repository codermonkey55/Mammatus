using System;

namespace Mammatus.Library.TimeZone
{
    /// <summary>
    /// A DateTime object in UTC, and a TimeZoneInfo object representing what timezone it's meant to target.
    /// </summary>
    public class DateTimeAndZone
    {
        /// <summary>
        /// The time stored, represented in UTC.
        /// </summary>
        public DateTime Utc { get; set; }

        /// <summary>
        /// The timezone this time originated from.
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; }


        protected DateTimeAndZone(DateTime utc, TimeZoneInfo timeZone)
        {
            if (utc.Kind == DateTimeKind.Local)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

            Utc = utc;
            TimeZone = timeZone;
        }

        public static DateTimeAndZone FromUtc(DateTime utc, TimeZoneInfo timeZone)
        {
            if (utc.Kind == DateTimeKind.Local)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

            return new DateTimeAndZone(utc, timeZone);
        }
        public static DateTimeAndZone FromUtcAndTimeZoneId(DateTime utc, TimeZoneId timeZoneId)
        {
            if (utc.Kind == DateTimeKind.Local)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

            return FromUtc(utc, TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
        }

        /// <summary>
        /// Returns a DateTimeAndZone no matter what. If timezoneShortname is null or not a real timezone,
        /// it returns UTC
        /// </summary>
        /// <param name="utc"></param>
        /// <param name="timezoneShortname"></param>
        /// <returns></returns>
        public static DateTimeAndZone FromUtcAndShort(DateTime utc, string timezoneShortname)
        {
            if (utc.Kind == DateTimeKind.Local)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

            if (timezoneShortname == null)
                return new DateTimeAndZone(utc, TimeZoneInfo.Utc);

            var tz = TimeZoneShortNameMap.Current.TimeZoneForShortName(timezoneShortname);

            if (tz == null)
                return new DateTimeAndZone(utc, TimeZoneInfo.Utc);

            return FromUtc(utc, tz);
        }

        /// <summary>
        /// Throws an exception if timezone is bad.
        /// </summary>
        /// <param name="utc"></param>
        /// <param name="timezoneShortname"></param>
        /// <returns></returns>
        public static DateTimeAndZone FromUtcAndShortStrict(DateTime utc, string timezoneShortname)
        {
            if (utc.Kind == DateTimeKind.Local)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Utc, not Local.", "utc");

            if (timezoneShortname == null)
                throw new ArgumentNullException("timezoneShortname");

            var tz = TimeZoneShortNameMap.Current.TimeZoneForShortName(timezoneShortname);

            if (tz == null)
                throw new ArgumentOutOfRangeException("timezoneShortname", timezoneShortname + " is not a mapped timezone.");

            return FromUtc(utc, tz);
        }

        public static DateTimeAndZone FromLocal(DateTime local, TimeZoneInfo timeZone)
        {
            if (local.Kind == DateTimeKind.Utc)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Unspecified, not Utc.", "local");

            // We need to push this date from Local (default for Parse) to Unspecified because it's outside
            // the limited timezone space the DateTime structure understands (Local, UTC, and go fish)
            if (local.Kind == DateTimeKind.Local)
                local = DateTime.SpecifyKind(local, DateTimeKind.Unspecified);

            var u = new DateTimeAndZone(TimeZoneInfo.ConvertTimeToUtc(local, timeZone), timeZone);
            return u;
        }
        public static DateTimeAndZone FromLocalAndTimeZoneId(DateTime local, TimeZoneId timeZoneId)
        {
            if (local.Kind == DateTimeKind.Utc)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Local, not Utc.", "local");

            return FromLocal(local, TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
        }
        public static DateTimeAndZone FromLocalAndShort(DateTime local, string timezoneShortname)
        {
            if (local.Kind == DateTimeKind.Utc)
                throw new ArgumentException("DateTime utc must be .Kind == DateTimeKind.Local, not Utc.", "local");

            return FromLocal(local, TimeZoneShortNameMap.Current.TimeZoneForShortName(timezoneShortname));
        }



        /// <summary>
        /// Gets a DateTime object mapped to the current timezone.
        /// </summary>
        public DateTime Local
        {
            get
            {
                var local = TimeZoneInfo.ConvertTimeFromUtc(Utc, this.TimeZone);
                return local;
            }
        }

        /// <summary>
        /// Whether this timezone is in daylight savings according to the TimeZoneInfo data.
        ///
        /// Note that weirdness with the TimeZoneInfo object means this will sometimes return true for timezones that have
        /// no Daylight Savings rules; if you're trying to determine if a timezone has a DST version, it may be simpler to
        /// check whether it has a DST shortname mapped.
        /// </summary>
        public bool IsDaylightSavings
        {
            get
            {
                return this.TimeZone.IsDaylightSavingTime(Utc);
            }
        }

        /// <summary>
        /// The short name for the timezone this date/timezone corresponds to, like PST, PDT, HKT.
        /// </summary>
        public string TimeZoneShortName
        {
            get
            {
                return TimeZoneShortNameMap.Current.ShortNameForTime(Utc, TimeZone);
            }
        }



        /// <summary>
        /// Switches this object to another timezone.
        /// </summary>
        /// <param name="shortTz"></param>
        public void SwitchTimeZone(string shortTz)
        {
            SwitchTimeZone(TimeZoneShortNameMap.Current.TimeZoneForShortName(shortTz));
        }
        public void SwitchTimeZone(TimeZoneId timeZoneId)
        {
            SwitchTimeZone(TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
        }
        public void SwitchTimeZone(TimeZoneInfo timeZoneInfo)
        {
            TimeZone = timeZoneInfo;
        }

        /// <summary>
        /// Gets a DateTime object for this time in the requested timezone.
        /// </summary>
        /// <param name="shortTz"></param>
        public DateTime ToLocal(string shortTz)
        {
            return ToLocal(TimeZoneShortNameMap.Current.TimeZoneForShortName(shortTz));
        }
        public DateTime ToLocal(TimeZoneId timeZoneId)
        {
            return ToLocal(TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
        }
        public DateTime ToLocal(TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(Utc, timeZoneInfo);
        }
    }
}
