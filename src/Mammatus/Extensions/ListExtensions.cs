using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mammatus.Extensions
{
    public static class ListExtensions
    {
        [ThreadStatic]
        static System.Random randomNumberGenerator = new Random(DateTime.Now.Millisecond + Thread.CurrentThread.GetHashCode());

        public static List<T> FromIndexToEnd<T>(this List<T> list, int start)
        {
            return list.GetRange(start, list.Count - start);
        }

        public static int IndexOf<T>(this IList<T> list, T target)
        {
            for (var i = 0; i < list.Count; ++i)
            {
                if (list[i].Equals(target)) return i;
            }
            return -1;
        }

        /// Returns a randomly selected item from IList<T>
        public static T RandomElement<T>(this IList<T> list)
        {
            if (list.IsEmpty()) throw new IndexOutOfRangeException("Cannot retrieve a random value from an empty list");

            return list[randomNumberGenerator.Next(list.Count)];
        }

        /// Returns a randomly selected item from IList<T> determined by a IEnumerable<float> of weights
        public static T RandomElement<T>(this IList<T> list, IEnumerable<float> weights)
        {
            if (list.IsEmpty()) throw new IndexOutOfRangeException("Cannot retrieve a random value from an empty list");
            if (list.Count != weights.Count()) throw new IndexOutOfRangeException("List of weights must be the same size as input list");

            var randomWeight = randomNumberGenerator.NextDouble() * weights.Sum();
            var totalWeight = 0f;
            var index = 0;
            foreach (var weight in weights)
            {
                totalWeight += weight;
                if (randomWeight <= totalWeight)
                {
                    break;
                }
            }

            return list[index];
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            // OrderBy and Sort are both broken for AOT compliation on older MonoTouch versions
            // https://bugzilla.xamarin.com/show_bug.cgi?id=2155#c11
            var shuffledList = new List<T>(list);
            T temp;
            for (var i = 0; i < shuffledList.Count; ++i)
            {
                temp = shuffledList[i];
                var swapIndex = randomNumberGenerator.Next(list.Count);
                shuffledList[i] = shuffledList[swapIndex];
                shuffledList[swapIndex] = temp;
            }
            return shuffledList;
        }

        public static IList<T> InPlaceShuffle<T>(this IList<T> list)
        {
            // OrderBy and Sort are both broken for AOT compliation on older MonoTouch versions
            // https://bugzilla.xamarin.com/show_bug.cgi?id=2155#c11

            for (var i = 0; i < list.Count; ++i)
            {
                var temp = list[i];
                var swapIndex = randomNumberGenerator.Next(list.Count);
                list[i] = list[swapIndex];
                list[swapIndex] = temp;
            }
            return list;
        }

        public static IList<T> InPlaceOrderBy<T, TKey>(this IList<T> list, Func<T, TKey> elementToSortValue) where TKey : IComparable
        {
            // Provides both and in-place sort as well as an AOT on iOS friendly replacement for OrderBy
            if (list.Count < 2)
            {
                return list;
            }

            int startIndex;
            int currentIndex;
            int smallestIndex;
            T temp;

            for (startIndex = 0; startIndex < list.Count; ++startIndex)
            {
                smallestIndex = startIndex;
                for (currentIndex = startIndex + 1; currentIndex < list.Count; ++currentIndex)
                {
                    if (elementToSortValue(list[currentIndex]).CompareTo(elementToSortValue(list[smallestIndex])) < 0)
                    {
                        smallestIndex = currentIndex;
                    }
                }
                temp = list[startIndex];
                list[startIndex] = list[smallestIndex];
                list[smallestIndex] = temp;
            }

            return list;
        }

        public static void InsertOrAdd<T>(this IList<T> list, int atIndex, T item)
        {
            if (atIndex >= 0 && atIndex < list.Count)
            {
                list.Insert(atIndex, item);
            }
            else
            {
                list.Add(item);
            }
        }

        public static T ElementAfter<T>(this IList<T> list, T element, bool wrap = true)
        {
            var targetIndex = list.IndexOf(element) + 1;
            if (wrap)
            {
                return targetIndex >= list.Count ? list[0] : list[targetIndex];
            }
            return list[targetIndex];
        }

        public static T ElementBefore<T>(this IList<T> list, T element, bool wrap = true)
        {
            var targetIndex = list.IndexOf(element) - 1;
            if (wrap)
            {
                return targetIndex < 0 ? list[list.Count - 1] : list[targetIndex];
            }
            return list[targetIndex];
        }
    }
}
