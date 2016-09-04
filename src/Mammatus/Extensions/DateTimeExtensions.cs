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
    }
}
