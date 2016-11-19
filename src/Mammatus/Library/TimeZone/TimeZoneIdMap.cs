using Mammatus.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Library.TimeZone
{
    public class TimeZoneIdMap
    {
        #region Singleton

        // http://codereview.stackexchange.com/questions/79/implementing-a-singleton-pattern-in-c
        public static TimeZoneIdMap Current => Nested.Instance;

        static class Nested
        {
            internal static readonly TimeZoneIdMap Instance = new TimeZoneIdMap();
        }

        #endregion

        protected Dictionary<TimeZoneId, string> enumToString = new Dictionary<TimeZoneId, string>();
        protected Dictionary<string, TimeZoneId> stringToEnum = new Dictionary<string, TimeZoneId>();

        protected TimeZoneIdMap()
        {
            StringValueAttribute.Map<TimeZoneId>(enumToString, stringToEnum);
        }

        public string IdToString(TimeZoneId timeZoneId)
        {
            return enumToString[timeZoneId];
        }

        public TimeZoneId StringToId(string timeZoneString)
        {
            return stringToEnum[timeZoneString];
        }

        public TimeZoneInfo FindSystemTimeZoneById(TimeZoneId timeZoneID)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(IdToString(timeZoneID));
        }

        public TimeZoneId TimeZoneInfoId(TimeZoneInfo tz)
        {
            return StringToId(tz.Id);
        }

        /// <summary>
        /// Retrieves a list of mapped TimeZoneIds, excluding TimeZoneId.None
        /// </summary>
        public TimeZoneId[] GetKnownTimeZoneIds()
        {
            var timeZoneIds = (TimeZoneId[])Enum.GetValues(typeof(TimeZoneId));
            return timeZoneIds.Where(t => t != TimeZoneId.None).ToArray();
        }
    }
}
