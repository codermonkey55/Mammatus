using System;
using System.Collections.Generic;

namespace Mammatus.Utils
{
    public class GenericComparer<T> : IComparer<T>
    {
        readonly Func<T, T, int> _compareFunction;

        public GenericComparer(Func<T, T, int> compareFunction)
        {
            _compareFunction = compareFunction;
        }

        public int Compare(T item1, T item2)
        {
            return _compareFunction(item1, item2);
        }
    }
}
