using System;
using System.Collections.Generic;

namespace Mammatus.Library.TimeZone
{
    public class TimeZoneByStateAndCountry
    {
        #region Singleton
        // http://codereview.stackexchange.com/questions/79/implementing-a-singleton-pattern-in-c
        public static TimeZoneByStateAndCountry Current { get { return Nested.instance; } }

        class Nested
        {
            static Nested()
            {
            }

            internal static readonly TimeZoneByStateAndCountry instance = new TimeZoneByStateAndCountry();
        }
        #endregion

        /// <summary>
        /// Maps state/country conjunct like "ca|us" to timezoneid like "Pacific Standard Time"
        /// </summary>
        protected Dictionary<string, TimeZoneId> stateCountryToTimeZoneId = new Dictionary<string, TimeZoneId>();

        public TimeZoneByStateAndCountry()
        {
            mapStatesAndCountriesToTimeZoneIds();
        }

        protected void mapStatesAndCountriesToTimeZoneIds()
        {
            // Builds the map of US and Canada timezones, and a few international.
            // TimeZoneInfo ID strings can be found in the Windows Registry at:
            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\

            var d = stateCountryToTimeZoneId;

            #region US
            d.Add("hi|us", TimeZoneId.HAST);

            d.Add("ak|us", TimeZoneId.AKST);

            // Pacific
            d.Add("wa|us", TimeZoneId.PST);
            d.Add("or|us", TimeZoneId.PST);
            d.Add("ca|us", TimeZoneId.PST);
            d.Add("nv|us", TimeZoneId.PST);

            // Mountain
            d.Add("mt|us", TimeZoneId.MST);
            d.Add("id|us", TimeZoneId.MST);
            d.Add("wy|us", TimeZoneId.MST);
            d.Add("ut|us", TimeZoneId.MST);
            d.Add("co|us", TimeZoneId.MST);
            d.Add("nm|us", TimeZoneId.MST);

            d.Add("az|us", TimeZoneId.AZMST);

            // Central
            d.Add("nd|us", TimeZoneId.CST);
            d.Add("sd|us", TimeZoneId.CST);
            d.Add("ne|us", TimeZoneId.CST);
            d.Add("ks|us", TimeZoneId.CST);
            d.Add("ok|us", TimeZoneId.CST);
            d.Add("tx|us", TimeZoneId.CST);
            d.Add("mn|us", TimeZoneId.CST);
            d.Add("ia|us", TimeZoneId.CST);
            d.Add("mo|us", TimeZoneId.CST);
            d.Add("ar|us", TimeZoneId.CST);
            d.Add("la|us", TimeZoneId.CST);
            d.Add("wi|us", TimeZoneId.CST);
            d.Add("il|us", TimeZoneId.CST);
            d.Add("tn|us", TimeZoneId.CST);
            d.Add("ms|us", TimeZoneId.CST);
            d.Add("al|us", TimeZoneId.CST);

            // Eastern
            d.Add("mi|us", TimeZoneId.EST);
            d.Add("oh|us", TimeZoneId.EST);
            d.Add("ky|us", TimeZoneId.EST);
            d.Add("ga|us", TimeZoneId.EST);
            d.Add("fl|us", TimeZoneId.EST);
            d.Add("me|us", TimeZoneId.EST);
            d.Add("vt|us", TimeZoneId.EST);
            d.Add("nh|us", TimeZoneId.EST);
            d.Add("ny|us", TimeZoneId.EST);
            d.Add("ma|us", TimeZoneId.EST);
            d.Add("ct|us", TimeZoneId.EST);
            d.Add("ri|us", TimeZoneId.EST);
            d.Add("pa|us", TimeZoneId.EST);
            d.Add("nj|us", TimeZoneId.EST);
            d.Add("de|us", TimeZoneId.EST);
            d.Add("wv|us", TimeZoneId.EST);
            d.Add("md|us", TimeZoneId.EST);
            d.Add("va|us", TimeZoneId.EST);
            d.Add("nc|us", TimeZoneId.EST);
            d.Add("sc|us", TimeZoneId.EST);

            d.Add("in|us", TimeZoneId.INEST);
            #endregion

            #region Canada
            d.Add("yt|ca", TimeZoneId.PST);
            d.Add("bc|ca", TimeZoneId.PST);

            d.Add("nt|ca", TimeZoneId.MST);
            d.Add("ab|ca", TimeZoneId.MST);

            d.Add("nu|ca", TimeZoneId.CST);
            d.Add("sk|ca", TimeZoneId.CST);
            d.Add("mb|ca", TimeZoneId.CST);

            d.Add("on|ca", TimeZoneId.EST);
            d.Add("qc|ca", TimeZoneId.EST);

            // Atlantic
            d.Add("ns|ca", TimeZoneId.AST);
            d.Add("pe|ca", TimeZoneId.AST);

            // Newfoundland
            d.Add("nl|ca", TimeZoneId.NST);
            #endregion

            #region World
            // Chile Standard Time / South American Western Standard Time
            d.Add("|cl", TimeZoneId.CLT);
            d.Add("|chile", TimeZoneId.CLT);

            d.Add("pr|us", TimeZoneId.AST);

            d.Add("|uk", TimeZoneId.GMT);

            // Central European Time
            d.Add("|fr", TimeZoneId.CET);
            d.Add("|france", TimeZoneId.CET);
            d.Add("|it", TimeZoneId.CET);
            d.Add("|italy", TimeZoneId.CET);

            d.Add("|uae", TimeZoneId.GST);

            d.Add("|hk", TimeZoneId.HKT);
            d.Add("|hong kong", TimeZoneId.HKT);
            d.Add("|cn", TimeZoneId.HKT);
            d.Add("|china", TimeZoneId.HKT);
            d.Add("|tw", TimeZoneId.HKT);
            d.Add("|taiwan", TimeZoneId.HKT);

            // West Pacific Standard Time / Chamorro Standard Time
            d.Add("|gu", TimeZoneId.ChST);
            d.Add("|guam", TimeZoneId.ChST);

            d.Add("samoa|ws", TimeZoneId.WST);
            #endregion
        }



        /// <summary>
        /// Returns a TimeZoneId from the Windows Registry, or null.
        /// Returns a best guess for the timezone of the state and country.
        /// 
        /// Matches state and country first, then just country. If both fail, returns null.
        /// </summary>
        /// <param name="state">A state like "CA" or "ON"</param>
        /// <param name="country">A country like "US", "CA", "FR", or "France"</param>
        /// <returns>The best timezone match, or the default timezone if none found</returns>
        public TimeZoneId GetTimeZoneId(string state, string country)
        {
            TimeZoneId timeZoneId;

            if (country != null)
            {
                country = country.ToLower();

                if (state != null)
                {
                    state = state.ToLower();
                    if (stateCountryToTimeZoneId.TryGetValue(state + "|" + country, out timeZoneId))
                        return timeZoneId;
                }

                if (stateCountryToTimeZoneId.TryGetValue("|" + country, out timeZoneId))
                    return timeZoneId;
            }

            return TimeZoneId.None;
        }


        public TimeZoneInfo GetTimeZoneInfo(string state, string country)
        {
            return GetTimeZoneInfo(state, country, TimeZoneId.None);
        }
        public TimeZoneInfo GetTimeZoneInfo(string state, string country, TimeZoneId defaultTimeZoneId)
        {
            var timeZoneId = GetTimeZoneId(state, country);
            if (timeZoneId == TimeZoneId.None)
            {
                if (defaultTimeZoneId == TimeZoneId.None)
                    return null;

                timeZoneId = defaultTimeZoneId;
            }

            return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneIdMap.Current.IdToString(timeZoneId));
        }

        public DateTimeAndZone GetUtcTimeZone(DateTime local, string state, string country)
        {
            var timeZone = GetTimeZoneInfo(state, country);
            if (timeZone == null)
                return null;
            return DateTimeAndZone.FromLocal(local, timeZone);
        }

        public DateTimeAndZone GetUtcTimeZone(DateTime local, string state, string country, TimeZoneId defaultTimeZoneId)
        {
            var timeZone = GetTimeZoneInfo(state, country, defaultTimeZoneId);
            return DateTimeAndZone.FromLocal(local, timeZone);
        }
    }
}
