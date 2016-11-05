using System;

namespace Mammatus.Extensions
{
    public static class FloatExtensions
    {
        public static float MapToRange(this float value, float range1Min, float range1Max, float range2Min, float range2Max)
        {
            return MapToRange(value, range1Min, range1Max, range2Min, range2Max, true);
        }

        public static float MapToRange(this float value, float range1Min, float range1Max, float range2Min, float range2Max, bool clamp)
        {

            value = range2Min + ((value - range1Min) / (range1Max - range1Min)) * (range2Max - range2Min);

            if (clamp)
            {
                if (range2Min < range2Max)
                {
                    if (value > range2Max) value = range2Max;
                    if (value < range2Min) value = range2Min;
                }
                // Range that go negative are possible, for example from 0 to -1
                else
                {
                    if (value > range2Min) value = range2Min;
                    if (value < range2Max) value = range2Max;
                }
            }
            return value;
        }

        public static int ToPercent(this float value)
        {
            return Convert.ToInt32(value * 100);
        }
    }
}
