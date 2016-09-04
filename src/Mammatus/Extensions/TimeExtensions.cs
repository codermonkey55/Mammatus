using Mammatus.Time;

namespace Mammatus.Extensions
{
    public static class TimeExtensions
    {

        public static DateTimeDiff Hours(this int offset)
        {
            return new DateTimeDiff(offset, (x, y) => x.AddHours(y));
        }

        public static DateTimeDiff Milliseconds(this int offset)
        {
            return new DateTimeDiff(offset, (x, y) => x.AddMilliseconds(y));
        }

        public static DateTimeDiff Minutes(this int offset)
        {
            return new DateTimeDiff(offset, (x, y) => x.AddMinutes(y));
        }

        public static DateTimeDiff Seconds(this int offset)
        {
            return new DateTimeDiff(offset, (x, y) => x.AddSeconds(y));
        }
    }
}
