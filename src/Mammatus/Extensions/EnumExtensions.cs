using System;

namespace Mammatus.Extensions
{
    public static class EnumExtensions
    {
        public static T Next<T>(this Enum enumValue)
        {
            var values = Enum.GetValues(enumValue.GetType());
            var totalNumberOfValues = values.Length;

            for (var i = 0; i < totalNumberOfValues; ++i)
            {
                if (values.GetValue(i).Equals(enumValue))
                {
                    return (i == totalNumberOfValues - 1)
                        ? (T)values.GetValue(0)
                        : (T)values.GetValue(i + 1);
                }
            }

            return default(T);
        }

        public static T Previous<T>(this Enum enumValue)
        {
            var values = Enum.GetValues(enumValue.GetType());
            var totalNumberOfValues = values.Length;

            for (var i = 0; i < totalNumberOfValues; ++i)
            {
                if (values.GetValue(i).Equals(enumValue))
                {
                    return (i == 0)
                        ? (T)values.GetValue(totalNumberOfValues - 1)
                        : (T)values.GetValue(i - 1);
                }
            }

            return default(T);
        }
    }
}
