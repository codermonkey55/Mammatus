/*
Copyright 2012-2013 Chris Moschini, Brass Nine Design

This code is licensed under the LGPL or MIT license, whichever you prefer.
*/

using System;
using System.Collections.Generic;

namespace Mammatus.Library.TimeZone
{
    public class TimeZoneShortNameMap
    {
        #region Singleton
        // http://codereview.stackexchange.com/questions/79/implementing-a-singleton-pattern-in-c
        public static TimeZoneShortNameMap Current { get { return Nested.instance; } }

        class Nested
        {
            static Nested()
            {
            }

            internal static readonly TimeZoneShortNameMap instance = new TimeZoneShortNameMap();
        }
        #endregion

        /// <summary>
        /// Maps short names like "PST" and "PDT" to timezone IDs in Windows Registry
        /// </summary>
        protected Dictionary<string, TimeZoneId> shortNameToTimeZoneId = new Dictionary<string, TimeZoneId>();

        /// <summary>
        /// Maps timezone IDs in Windows Registry to short names, like "Pacific Standard Time" to "PST"
        /// </summary>
        protected Dictionary<TimeZoneId, string> timeZoneIdToStandardShortName = new Dictionary<TimeZoneId, string>();

        /// <summary>
        /// Maps timeZoneIds in Windows Registry to short daylight names, like "Pacific Standard Time" to "PDT"
        /// If there is no daylight name, nothing mapped - fall back to timeZoneIdToStandardShortName
        /// </summary>
        protected Dictionary<TimeZoneId, string> timeZoneIdToDaylightShortName = new Dictionary<TimeZoneId, string>();



        public TimeZoneShortNameMap()
        {
            mapTimeZoneIdsAndShortNames();
        }

        protected void mapTimeZoneIdsAndShortNames()
        {
            mapTimeZoneIdAndShortName(TimeZoneId.UTC, "UTC");
            mapTimeZoneIdAndShortName(TimeZoneId.HAST, "HAST", "HADT");     // Hawaii-Aleutian Standard Time
            mapTimeZoneIdAndShortName(TimeZoneId.AKST, "AKST", "AKDT");     // Alaska
            mapTimeZoneIdAndShortName(TimeZoneId.PST, "PST", "PDT");        // Pacific
            mapTimeZoneIdAndShortName(TimeZoneId.MST, "MST", "MDT");        // Mountain
            mapTimeZoneIdAndShortName(TimeZoneId.AZMST, "AZMST");           // Arizona - never DST
            mapTimeZoneIdAndShortName(TimeZoneId.CST, "CST", "CDT");        // Central
            mapTimeZoneIdAndShortName(TimeZoneId.EST, "EST", "EDT");        // Eastern
            mapTimeZoneIdAndShortName(TimeZoneId.INEST, "INEST", "INEDT");  // Indiana began observing EDT in 2006
            mapTimeZoneIdAndShortName(TimeZoneId.AST, "AST", "ADT");        // Atlantic
            mapTimeZoneIdAndShortName(TimeZoneId.NST, "NST", "NDT");        // Newfoundland
            mapTimeZoneIdAndShortName(TimeZoneId.CLT, "CLT", "CLST");       // Chilean Summer Time
            mapTimeZoneIdAndShortName(TimeZoneId.GMT, "GMT", "BST");        // Greenwich Mean / British Summer Time
            mapTimeZoneIdAndShortName(TimeZoneId.CET, "CET", "CEST");       // Central European / Summer
            mapTimeZoneIdAndShortName(TimeZoneId.GST, "GST");               // Gulf / Arabian
            mapTimeZoneIdAndShortName(TimeZoneId.HKT, "HKT");               // Hong Kong Time; aka "CST" China Standard Time
            mapTimeZoneIdAndShortName(TimeZoneId.ChST, "ChST");             // Chamorro (Guam) Standard Time
            mapTimeZoneIdAndShortName(TimeZoneId.WST, "WST");               // West Samoa Time
        }



        protected void mapTimeZoneIdAndShortName(TimeZoneId timeZoneId, string shortStandard)
        {
            mapTimeZoneIdAndShortName(timeZoneId, shortStandard, null);
        }
        protected void mapTimeZoneIdAndShortName(TimeZoneId timeZoneId, string shortStandard, string shortDaylight)
        {
            shortNameToTimeZoneId.Add(shortStandard, timeZoneId);
            timeZoneIdToStandardShortName.Add(timeZoneId, shortStandard);

            if (shortDaylight != null)
            {
                shortNameToTimeZoneId.Add(shortDaylight, timeZoneId);
                timeZoneIdToDaylightShortName.Add(timeZoneId, shortDaylight);
            }
        }


        public string ShortNameForTime(DateTime utc, TimeZoneId timeZoneId)
        {
            return ShortNameForTime(utc, TimeZoneIdMap.Current.FindSystemTimeZoneById(timeZoneId));
        }
        public string ShortNameForTime(DateTime utc, TimeZoneInfo tz)
        {
            TimeZoneId timeZoneId = TimeZoneIdMap.Current.TimeZoneInfoId(tz);
            string shortName;
            if (tz.IsDaylightSavingTime(utc) && timeZoneIdToDaylightShortName.TryGetValue(timeZoneId, out shortName))
                return shortName;

            return timeZoneIdToStandardShortName[timeZoneId];
        }

        public string StandardNameForTimeZoneId(TimeZoneId timeZoneId)
        {
            return timeZoneIdToStandardShortName[timeZoneId];
        }
        public string DaylightNameForTimeZoneId(TimeZoneId timeZoneId)
        {
            string daylight = null;
            timeZoneIdToDaylightShortName.TryGetValue(timeZoneId, out daylight);
            return daylight;
        }

        public TimeZoneInfo TimeZoneForShortName(string shortName)
        {
            if (shortName == null)
                throw new ArgumentNullException("shortName");

            string timeZoneId = TimeZoneIdStringForShortName(shortName);
            if (timeZoneId == null)
                return null;
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        public TimeZoneId TimeZoneIdForShortName(string shortName)
        {
            if (shortName == null)
                throw new ArgumentNullException("shortName");

            TimeZoneId timeZoneId = TimeZoneId.None;
            shortNameToTimeZoneId.TryGetValue(shortName, out timeZoneId);
            return timeZoneId;
        }

        public string TimeZoneIdStringForShortName(string shortName)
        {
            var timeZoneId = TimeZoneIdForShortName(shortName);
            if (timeZoneId == TimeZoneId.None)
                return null;

            return TimeZoneIdMap.Current.IdToString(timeZoneId);
        }
    }
}
