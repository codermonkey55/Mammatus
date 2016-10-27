using Mammatus.Extensions;
using Mammatus.IO.Enums;
using System;

namespace Mammatus.Numeric.Extensions
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
    }
}
