using Mammatus.IO.Enums;
using System;

namespace Mammatus.Extensions
{
    public static class NumericExtensions
    {
        public static string ToFileSize(this int size)
        {
            if (size < FileSize.KiloByte.To<int>())
                return size + " Bytes";

            else if (size < FileSize.MegaByte.To<int>())
                return ((Double)size / FileSize.KiloByte.To<int>()).ToString("0.## KB");

            else if (size < FileSize.GigaByte.To<int>())
                return ((Double)size / FileSize.MegaByte.To<int>()).ToString("0.## MB");

            else if (size < FileSize.TeraByte.To<int>())
                return ((Double)size / FileSize.GigaByte.To<int>()).ToString("0.## GB");

            else
                return ((Double)size / FileSize.TeraByte.To<int>()).ToString("0.## TB");
        }

        public static void Times(this int iterations, Action callback)
        {
            for (var i = 0; i < iterations; ++i)
            {
                callback();
            }
        }

        public static void Times(this int iterations, Action<int> callback)
        {
            for (var i = 0; i < iterations; ++i)
            {
                callback(i);
            }
        }

        public static void UpTo(this int value, int endValue, Action<int> callback)
        {
            for (var i = value; i <= endValue; ++i)
            {
                callback(i);
            }
        }

        public static void DownTo(this int value, int endValue, Action<int> callback)
        {
            for (var i = value; i >= endValue; --i)
            {
                callback(i);
            }
        }

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 == 1;
        }
    }
}
