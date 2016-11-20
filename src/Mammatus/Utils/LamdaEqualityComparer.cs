using System;
using System.Collections.Generic;

namespace Mammatus.Utils
{
    internal class LamdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _compareFunction;
        private readonly Func<T, int> _hashFunction;

        public LamdaEqualityComparer(Func<T, T, bool> compareFunction, Func<T, int> hashFunction)
        {
            this._compareFunction = compareFunction;
            this._hashFunction = hashFunction;
        }

        public bool Equals(T x, T y)
        {
            return _compareFunction(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hashFunction(obj);
        }

        public static LamdaEqualityComparer<T> Create<TValue>(Func<T, TValue> projection)
        {
            return new LamdaEqualityComparer<T>(
                (t1, t2) => EqualityComparer<TValue>.Default.Equals(projection(t1), projection(t2)),
                t => EqualityComparer<TValue>.Default.GetHashCode(projection(t)));
        }
    }
}
