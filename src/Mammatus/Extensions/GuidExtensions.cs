using System;

namespace Mammatus.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsEmptyOrNull(this System.Guid value)
        {
            return __IsEmptyOrNull(value);
        }

        public static bool IsEmptyOrNull(this System.Guid? value)
        {
            return __IsEmptyOrNull(value);
        }

        public static bool IsValid(this System.Guid value)
        {
            byte[] bits = value.ToByteArray();

            const int variantShift = 6;
            const int variantMask = 0x3 << variantShift;
            const int variantBits = 0x2 << variantShift;

            if ((bits[8] & variantMask) != variantBits)
            {
                return false;
            }

            const int versionShift = 4;
            const int versionMask = 0xf << versionShift;
            const int versionBits = 0x4 << versionShift;

            if ((bits[7] & versionMask) != versionBits)
            {
                return false;
            }

            return true;
        }

        public static System.Guid NewCombGuid()
        {
            var baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();

            // Get the days and milliseconds which will be used to build the byte string
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            var msecs = new TimeSpan(now.Ticks - new DateTime(now.Year, now.Month, now.Day).Ticks);

            // Convert to a byte array
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new System.Guid(guidArray);
        }

        public static System.Guid NewCombGuid(this System.Guid value)
        {
            return NewCombGuid();
        }

        public static DateTime ShowDate(this System.Guid value)
        {
            var dayBits = new byte[4];
            var msecBits = new byte[4];
            byte[] guidBits = value.ToByteArray();

            // First we should obtain msecs and days from guid hex string
            Array.Copy(guidBits, guidBits.Length - 6, dayBits, 2, 2);
            Array.Copy(guidBits, guidBits.Length - 4, msecBits, 0, 4);

            // Now we need to reverse both arrays into an appropiate order
            // so we can work with them.. remember Intel (and .Net) is little endian ;)
            Array.Reverse(dayBits);
            Array.Reverse(msecBits);

            // Now that we have days in a byte array, we can convert them into something
            // more useable.. let's say a TimeSpam objet..
            var days = new TimeSpan(BitConverter.ToInt32(dayBits, 0), 0, 0, 0);
            var date = new DateTime(new DateTime(1900, 1, 1).Ticks + days.Ticks);

            // This should print the year/month/day, but at 00:00:00 hours.

            // Now we need to get the hour.. it is encoded in milliseconds with
            // 1/300th aproximation.. so first, let's pass it to a double
            // and multiply it by 3.33333
            double tmp = BitConverter.ToInt32(msecBits, 0) * 3.333333;

            // Now we can convert this into a TimeSpan, passing milliseconds
            // as Ticks. Remeber, ticks is a "normally" a constant value
            // witch 10000/1 times a second. But you should not assume this value
            // but instead you should use TimeSpan.TicksPerMillisecond constant
            // to operate with ticks vs msecs.
            var msecs = new TimeSpan(((long)tmp) * TimeSpan.TicksPerMillisecond);

            // Now we can obtain the final date
            date = new DateTime(date.Ticks + msecs.Ticks);

            return date;
        }

        private static bool __IsEmptyOrNull(this System.Guid? value)
        {
            if (value == null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            if (value == new System.Guid())
            {
                return true;
            }

            return false;
        }
    }
}