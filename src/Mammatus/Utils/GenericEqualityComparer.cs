using System;
using System.Collections.Generic;

namespace Mammatus.Utils
{
    public class GenericEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        private readonly Func<T, object> _expression;

        protected GenericEqualityComparer(Func<T, object> expr)
        {
            this._expression = expr;
        }

        public bool Equals(T x, T y)
        {
            var first = _expression.Invoke(x);
            var sec = _expression.Invoke(y);
            if (first != null && first.Equals(sec))
                return true;
            else
                return false;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public static GenericEqualityComparer<T> Create(Func<T, object> projection)
        {
            return new GenericEqualityComparer<T>(projection);
        }
    }
}
