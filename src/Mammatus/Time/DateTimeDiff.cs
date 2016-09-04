using System;

namespace Mammatus.Time
{
    public sealed class DateTimeDiff
    {
        public int Offset { get; private set; }
        public Func<DateTime, int, DateTime> Calculation { get; private set; }

        public DateTimeDiff(int offset, Func<DateTime, int, DateTime> calculation)
        {
            Offset = offset;
            Calculation = calculation;
        }

        public DateTime After(DateTime date)
        {
            return Calculation(date, Offset);
        }

        public DateTime Before(DateTime date)
        {
            return Calculation(date, -Offset);
        }
    }
}
