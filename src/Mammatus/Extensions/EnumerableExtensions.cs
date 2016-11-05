using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mammatus.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            var items = source as T[] ?? source?.ToArray();
            if (items != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
            return items;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var items = source as T[] ?? source.ToArray();
            if (items != null)
            {
                var index = 0;
                foreach (var item in items)
                {
                    action(item, index);
                    index++;
                }
            }
            return items;
        }

        public static T RandomOne<T>(this IEnumerable<T> source)
        {
            return source.RandomOneUsing(new Random());
        }

        public static T RandomOneUsing<T>(this IEnumerable<T> source, Random rand)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int index = rand.Next(0, source.Count());
            return source.ElementAt(index);
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int take)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            IList<T> list = source as IList<T> ?? source.ToArray();
            if (take >= list.Count)
                return list;

            var rand = new Random();
            var items = new HashSet<T>();
            while (items.Count < take)
            {
                var i = rand.Next(0, list.Count);
                items.Add(list[i]);
            }
            return items;
        }

        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> selector)
        {
            int index = -1;
            if (source != null)
            {
                var i = 0;
                foreach (var item in source)
                {
                    if (selector(item))
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
            }
            return index;
        }

        public static IEnumerable<T> Union<T>(this IEnumerable<T> source, params T[] parameters)
        {
            if (parameters != null)
            {
                return source.Union(parameters);
            }
            return source;
        }

        public static IEnumerable<T> UnionBefore<T>(this IEnumerable<T> source, params T[] parameters)
        {
            if (parameters != null)
            {
                return parameters.Union(source);
            }
            return source;
        }

        public static IList<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>(enumerable.ToList());
        }

        public static T FindMinElement<T>(this IEnumerable<T> list, Converter<T, int> projection)
        {
            var enumerable = list as T[] ?? list.ToArray();

            if (enumerable.Any() == false)
            {
                throw new InvalidOperationException("Empty list");
            }

            int minValue = int.MinValue;
            T minElement = default(T);

            foreach (T item in enumerable)
            {
                int value = projection(item);
                if (value < minValue)
                {
                    minValue = value;
                    minElement = item;
                }
            }
            return minElement;
        }

        public static T FindMaxElement<T>(this IEnumerable<T> list, Converter<T, int> projection)
        {
            var enumerable = list as T[] ?? list.ToArray();

            if (enumerable.Any() == false)
            {
                throw new InvalidOperationException("Empty list");
            }

            int maxValue = int.MinValue;
            T maxElement = default(T);

            foreach (T item in enumerable)
            {
                int value = projection(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxElement = item;
                }
            }
            return maxElement;
        }

        public static void Zip<T, U>(this IEnumerable<T> iterable1, IEnumerable<U> iterable2, Action<T, U> callback)
        {
            var i1Enumerator = iterable1.GetEnumerator();
            var i2Enumerator = iterable2.GetEnumerator();

            while (i1Enumerator.MoveNext() && i2Enumerator.MoveNext())
            {
                callback(i1Enumerator.Current, i2Enumerator.Current);
            }
        }

        public static void Zip(this IEnumerable iterable1, IEnumerable iterable2, Action<object, object> callback)
        {
            var i1Enumerator = iterable1.GetEnumerator();
            var i2Enumerator = iterable2.GetEnumerator();

            while (i1Enumerator.MoveNext() && i2Enumerator.MoveNext())
            {
                callback(i1Enumerator.Current, i2Enumerator.Current);
            }
        }

        public static void InParallelWith<T, U>(this IEnumerable<T> iterable1, IEnumerable<U> iterable2, Action<T, U> callback)
        {
            if (iterable1.Count() != iterable2.Count()) throw new ArgumentException(string.Format("Both IEnumerables must be the same length, iterable1: {0}, iterable2: {1}", iterable1.Count(), iterable2.Count()));

            var i1Enumerator = iterable1.GetEnumerator();
            var i2Enumerator = iterable2.GetEnumerator();

            while (i1Enumerator.MoveNext())
            {
                i2Enumerator.MoveNext();
                callback(i1Enumerator.Current, i2Enumerator.Current);
            }
        }

        public static void InParallelWith(this IEnumerable iterable1, IEnumerable iterable2, Action<object, object> callback)
        {
            var i1Enumerator = iterable1.GetEnumerator();
            var i2Enumerator = iterable2.GetEnumerator();
            var i1Count = 0;
            var i2Count = 0;
            while (i1Enumerator.MoveNext()) ++i1Count;
            while (i2Enumerator.MoveNext()) ++i2Count;
            if (i1Count != i2Count) throw new ArgumentException(string.Format("Both IEnumerables must be the same length, iterable1: {0}, iterable2: {1}", i1Count, i2Count));

            i1Enumerator.Reset();
            i2Enumerator.Reset();
            while (i1Enumerator.MoveNext())
            {
                i2Enumerator.MoveNext();
                callback(i1Enumerator.Current, i2Enumerator.Current);
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> iterable)
        {
            return iterable.Count() == 0;
        }

        public static bool IsEmpty(this IEnumerable iterable)
        {
            // MoveNext returns false if we are at the end of the collection
            return !iterable.GetEnumerator().MoveNext();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> iterable)
        {
            return iterable.Count() > 0;
        }

        public static bool IsNotEmpty(this IEnumerable iterable)
        {
            // MoveNext returns false if we are at the end of the collection
            return iterable.GetEnumerator().MoveNext();
        }
    }
}
