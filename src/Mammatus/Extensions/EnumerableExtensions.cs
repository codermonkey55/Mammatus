using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mammatus.Enumerable.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
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

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var items = source as T[] ?? source.ToArray();
            if (source != null)
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
    }
}
