using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mammatus.Library.Dynamic;

namespace Mammatus.Enumerable.Extensions
{
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ReadOnlyForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            var items = source as T[] ?? source.ToArray();
            if (source != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
            return items;
        }

        public static IEnumerable<T> ArrayEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                var items = source as T[] ?? source.ToArray();

                for (int i = 0; i < items.Length; i++)
                {
                    action(items[i]);
                }

                source = items;
            }
            return source;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source != null)
            {
                var items = source as T[] ?? source.ToArray();

                var index = 0;
                foreach (var item in items)
                {
                    action(item, index);
                    index++;
                }

                source = items;
            }
            return source;
        }

        public static T RandomOne<T>(this IEnumerable<T> source)
        {
            return source.RandomOneUsing(new Random());
        }

        public static T RandomOneUsing<T>(this IEnumerable<T> source, Random rand)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var enumerable = source as T[] ?? source.ToArray();

            int index = rand.Next(0, enumerable.Count());

            return enumerable.ElementAt(index);
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int take)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

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

        /// <summary>
        /// See for more details, http://stackoverflow.com/questions/3464934/get-max-value-from-listmytype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
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

        /// <summary>
        /// See for more details, http://stackoverflow.com/questions/3464934/get-max-value-from-listmytype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
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

        public static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> selector)
        {
            foreach (T item in source)
            {
                if (selector(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<T> Distinct<T, TResult>(this IEnumerable<T> source, Func<T, TResult> comparer)
        {
            return source.Distinct(new DynamicComparer<T, TResult>(comparer));
        }

        public static IEnumerable<T> Alternate<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            using (IEnumerator<T> e1 = first.GetEnumerator())
            {
                using (IEnumerator<T> e2 = second.GetEnumerator())
                {
                    while (e1.MoveNext() && e2.MoveNext())
                    {
                        yield return e1.Current;
                        yield return e2.Current;
                    }
                }
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T element)
        {
            foreach (var item in source)
            {
                yield return item;
            }

            yield return element;
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T element)
        {
            yield return element;

            foreach (var item in source)
            {
                yield return item;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ForEach(source, (i, item) => action(item));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<int, T> action)
        {
            int i = 0;

            foreach (var item in source)
            {
                action(i++, item);
            }
        }

        public static IEnumerable<TTarget> ForEach<T, TTarget>(this IEnumerable<T> source, Func<T, TTarget> func)
        {
            return ForEach(source, (i, item) => func(item));
        }

        public static IEnumerable<TTarget> ForEach<T, TTarget>(this IEnumerable<T> source, Func<int, T, TTarget> func)
        {
            int i = 0;

            foreach (var item in source)
            {
                yield return func(i++, item);
            }
        }





        public static Dictionary<TKey, TValue> Merge<TKey, TValue>
            (this Dictionary<TKey, TValue> first, IEnumerable<KeyValuePair<TKey, TValue>> second)
        {
            if (first == null && second == null)
            {
                return null;
            }
            else if (first == null)
            {
                return second.ToDictionary(x => x.Key, x => x.Value);
            }
            else if (second == null)
            {
                return first;
            }

            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            foreach (var x in first.Concat(second).Where(x => x.Value != null))
            {
                dictionary[x.Key] = x.Value;
            }

            return dictionary;
        }

        public static ConcurrentDictionary<TKey, TValue> Merge<TKey, TValue>
            (this ConcurrentDictionary<TKey, TValue> first, IEnumerable<KeyValuePair<TKey, TValue>> second)
        {
            if (first == null && second == null)
            {
                return null;
            }
            else if (first == null)
            {
                return new ConcurrentDictionary<TKey, TValue>(second);
            }
            else if (second == null)
            {
                return first;
            }

            ConcurrentDictionary<TKey, TValue> dictionary = new ConcurrentDictionary<TKey, TValue>();

            foreach (var x in first.Concat(second).Where(x => x.Value != null))
            {
                dictionary[x.Key] = x.Value;
            }

            return dictionary;
        }

        public static T? FirstOrNull<T>(this IEnumerable<T> source) where T : struct
        {
            foreach (var item in source)
            {
                return item;
            }
            return null;
        }

        public static T? FirstOrNull<T>
            (this IEnumerable<T> source, Func<T, bool> predicate) where T : struct
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return null;
        }


        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> souce, int count)
        {
            return Enumerable.Range(0, count).SelectMany(_ => souce);
        }

        public static IEnumerable<T> Stretch<T>(this IEnumerable<T> souce, int count)
        {
            return souce.SelectMany(value => Enumerable.Range(0, count).Select(_ => value));
        }

        public static IList<T> Imitate<T>
            (this IList<T> source, IEnumerable<T> reference) where T : class
        {
            return source.Imitate(reference, object.ReferenceEquals, x => x);
        }

        public static IList<T> Imitate<T>
            (this IList<T> source, IEnumerable<T> reference, Func<T, T, bool> match)
        {
            return source.Imitate(reference, match, x => x);
        }

        public static IList<T1> Imitate<T1, T2>
            (this IList<T1> source, IEnumerable<T2> reference, Func<T1, T2, bool> match, Func<T2, T1> converter)
        {
            var enumerable = reference as T2[] ?? reference.ToArray();

            source.Absorb(enumerable, match, converter);

            return source.FilterStrictlyBy(enumerable, match);
        }

        public static void AbsorbStrictly<T1, T2>(this IList<T1> source, IEnumerable<T2> reference, Func<T1, T2, bool> match, Func<T2, T1> converter)
        {
            int startIndex = 0;

            foreach (var item in reference)
            {
                var length = source.Count;

                if (startIndex >= length)
                {
                    source.Add(converter(item));
                    startIndex = source.Count;
                    continue;
                }

                if (!match(source[startIndex], item))
                {
                    source.Insert(startIndex, converter(item));
                }
                startIndex++;
            }
        }

        public static void Absorb<T1, T2>(this IList<T1> source, IEnumerable<T2> reference, Func<T1, T2, bool> match, Func<T2, T1> converter)
        {
            int startIndex = 0;

            foreach (var item in reference)
            {

                var length = source.Count;
                var existance = false;

                for (int i = startIndex; i < length; i++)
                {
                    var checkItem = source[i];

                    if (match(checkItem, item))
                    {
                        existance = true;
                        startIndex = i + 1;
                        break;
                    }
                }

                if (!existance)
                {
                    if (startIndex >= length)
                    {
                        source.Add(converter(item));
                        startIndex = source.Count;
                    }
                    else
                    {
                        source.Insert(startIndex, converter(item));
                        startIndex++;
                    }
                }
            }
        }

        private static List<T1> FilterStrictlyBy<T1, T2>(this IList<T1> source, IEnumerable<T2> reference, Func<T1, T2, bool> match)
        {
            var removedItems = new List<T1>();

            using (var e = reference.GetEnumerator())
            {
                var usable = e.MoveNext();
                var currentReference = e.Current;

                foreach (var item in source)
                {
                    if (!usable || !match(item, currentReference))
                    {
                        removedItems.Add(item);
                    }
                    else
                    {
                        usable = e.MoveNext();
                        currentReference = e.Current;
                    }
                }
            }


            foreach (var di in removedItems)
            {
                source.Remove(di);
            }
            return removedItems;
        }

        public static List<T1> FilterBy<T1, T2>(this IList<T1> source, IEnumerable<T2> reference, Func<T1, T2, bool> match)
        {
            int referenceIndex = 0;

            var removedItems = new List<T1>();

            foreach (var item in source)
            {
                var enumerable = reference as T2[] ?? reference.ToArray();

                var existingIndex = enumerable.FindIndex(referenceIndex, x => match(item, x));

                if (existingIndex < 0)
                {
                    removedItems.Add(item);
                }
                else
                {
                    referenceIndex = existingIndex + 1;
                }

            }

            foreach (var di in removedItems)
            {
                source.Remove(di);
            }
            return removedItems;
        }

        public static int FindIndex<T>(this IEnumerable<T> source, Predicate<T> match)
        {
            return source.FindIndex(0, match);
        }

        public static int FindIndex<T>(this IEnumerable<T> source, int startIndex, Predicate<T> match)
        {
            int index = 0;
            foreach (var x in source)
            {
                if (index >= startIndex && match(x))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static int FindLastIndex<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            var index = 0;
            var result = -1;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    result = index;
                }
                index++;
            }
            return result;
        }

        public static int BinarySearch<T>(this IList<T> source, Predicate<T> match)
        {
            return source.BinarySearch(match, 0, source.Count - 1, false);
        }

        public static int BinarySearchReversed<T>(this IList<T> source, Predicate<T> match)
        {
            return source.BinarySearch(match, 0, source.Count - 1, true);
        }

        public static int BinarySearch<T>(this IList<T> source, Predicate<T> match,
            int startIndex, int lastIndex, bool reversed)
        {
            if (!reversed && !match(source[lastIndex]))
            {
                return -1;
            }
            if (reversed && !match(source[startIndex]))
            {
                return -1;
            }

            var left = startIndex;
            var right = lastIndex;


            while (left < right)
            {
                var mid = (left + right) / 2;

                if (match(source[mid]) ^ (!reversed))
                {
                    left = mid;
                }
                else
                {
                    right = mid;
                }

                if (left == right)
                {
                    return left;
                }
                else if (left + 1 == right)
                {
                    if (!reversed)
                    {
                        return match(source[left]) ? left : right;
                    }
                    else
                    {
                        return match(source[right]) ? right : left;
                    }
                }
            }
            return -1;
        }

        public static IEnumerable<double> Integral(this IEnumerable<double> source)
        {
            double sum = 0.0;

            foreach (var item in source)
            {
                sum += item;
                yield return sum;
            }
        }

        public static IEnumerable<int> Integral(this IEnumerable<int> source)
        {
            int sum = 0;

            foreach (var item in source)
            {
                sum += item;
                yield return sum;
            }
        }

        public static IEnumerable<long> Integral(this IEnumerable<long> source)
        {
            long sum = 0;

            foreach (var item in source)
            {
                sum += item;
                yield return sum;
            }
        }

        public static bool SequenceEqual<T1, T2>
            (this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, bool> match)
        {
            if (first == null && second == null)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (!(e2.MoveNext() && match(e1.Current, e2.Current))) return false;
                }
                if (e2.MoveNext()) return false;
            }
            return true;
        }

        public static bool SequenceEqualParallel<T1, T2>
            (this IList<T1> first, IList<T2> second, Func<T1, T2, bool> match)
        {
            if (first == null && second == null)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            if (first.Count != second.Count)
            {
                return false;
            }

            return first.Zip(second, (a, b) => new Tuple<T1, T2>(a, b))
                .AsParallel()
                .All(x => match(x.Item1, x.Item2));

        }

        public static bool ContainsIndex<T>(this IList<T> list, int index)
        {
            if (list == null)
            {
                return false;
            }
            return (index >= 0 && index < list.Count);
        }

        public static bool ContainsIndex<T>(this T[] array, int index)
        {
            if (array == null)
            {
                return false;
            }
            return (index >= 0 && index < array.Length);
        }

        public static T FromIndexOrDefault<T>(this IList<T> list, int index) => (list.ContainsIndex(index)) ? list[index] : default(T);

        public static T FromIndexOrDefault<T>(this T[] array, int index)
        {
            if (array.ContainsIndex(index))
            {
                return array[index];
            }
            return default(T);
        }

        public static IEnumerable<TOut> TakeOver<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TIn, TOut> func)
        {
            var enumerable = source as TIn[] ?? source.ToArray();

            return enumerable.TakeOver(func, enumerable.First());
        }

        public static IEnumerable<TOut> TakeOver<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TIn, TOut> func, TIn initialValue)
        {
            //yield return source.First() - initialValue;
            var prev = initialValue;
            foreach (var value in source)
            {
                yield return func(prev, value);
                prev = value;
            }
        }

        public static IEnumerable<object> AsEnumerable(this IEnumerable array)
        {
            foreach (var item in array)
            {
                yield return item;
            }
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return (list == null || list.Count <= 0);
        }

        public static bool IsNullOrEmpty<T>(this T[] list)
        {
            return (list == null || list.Length <= 0);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, params T[] toAppend)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return Enumerable.Concat(sequence, toAppend);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> sequence, params T[] toPrepend)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return Enumerable.Concat(toPrepend, sequence);
        }
    }
}
