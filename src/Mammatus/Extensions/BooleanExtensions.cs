using System;

namespace Mammatus.Extensions
{
    public static class BooleanExtensions
    {
        public static bool IfTrue(this bool decision, Action action)
        {
            if (decision)
            {
                action();
            }

            return decision;
        }

        public static bool IfFalse(this bool decision, Action action)
        {
            return IfTrue(!decision, action);
        }

        public static bool IfTrue<T>(this bool decision, Action<T> action, T param)
        {
            if (decision)
            {
                action(param);
            }

            return decision;
        }

        public static bool IfFalse<T>(this bool decision, Action<T> action, T param)
        {
            return IfTrue(!decision, action, param);
        }

        public static bool IfTrue<T1, T2>(this bool decision, Action<T1, T2> action, T1 param1, T2 param2)
        {
            if (decision)
            {
                action(param1, param2);
            }

            return decision;
        }

        public static bool IfFalse<T1, T2>(this bool decision, Action<T1, T2> action, T1 param1, T2 param2)
        {
            return IfTrue(!decision, action, param1, param2);
        }
    }
}
