using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mammatus.Extensions
{
    public static class ArrayExtensions
    {
        [ThreadStatic]
        static System.Random randomNumberGenerator = new Random(DateTime.Now.Millisecond + Thread.CurrentThread.GetHashCode());

        public static int IndexOf<T>(this T[] array, T target)
        {
            for (var i = 0; i < array.Length; ++i)
            {
                if (array[i].Equals(target)) return i;
            }
            return -1;
        }
        public static T[] FromIndexToEnd<T>(this T[] array, int start)
        {
            var subSection = new T[array.Length - start];
            array.CopyTo(subSection, start);
            return subSection;
        }

        public static int FindIndex<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindIndex(array, match);
        }

        public static int FindIndex<T>(this T[] array, int startIndex, Predicate<T> match)
        {
            return Array.FindIndex(array, startIndex, match);
        }

        public static int FindIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        {
            return Array.FindIndex(array, startIndex, count, match);
        }

        /// Returns a randomly selected item from the array
        public static T RandomElement<T>(this T[] array)
        {
            if (array.Length == 0) throw new IndexOutOfRangeException("Cannot retrieve a random value from an empty array");

            return array[randomNumberGenerator.Next(array.Length)];
        }

        /// Returns a randomly selected item from the array determined by a float array of weights
        public static T RandomElement<T>(this T[] array, float[] weights)
        {
            return array.RandomElement(weights.ToList());
        }

        /// Returns a randomly selected item from the array determined by a List<float> of weights
        public static T RandomElement<T>(this T[] array, List<float> weights)
        {
            if (array.IsEmpty()) throw new IndexOutOfRangeException("Cannot retrieve a random value from an empty array");
            if (array.Count() != weights.Count()) throw new IndexOutOfRangeException("array of weights must be the same size as input array");

            var randomWeight = randomNumberGenerator.NextDouble() * weights.Sum();
            var totalWeight = 0f;
            var index = weights.FindIndex(weight =>
            {
                totalWeight += weight;
                return randomWeight <= totalWeight;
            });

            return array[index];
        }

        public static void EachWithIndex<T>(this T[,] collection, Action<T, int, int> callback)
        {
            for (var x = 0; x < collection.GetLength(0); ++x)
            {
                for (var y = 0; y < collection.GetLength(1); ++y)
                {
                    callback(collection[x, y], x, y);
                }
            }
        }

        public static void EachIndex<T>(this T[,] collection, Action<int, int> callback)
        {
            for (var x = 0; x < collection.GetLength(0); ++x)
            {
                for (var y = 0; y < collection.GetLength(1); ++y)
                {
                    callback(x, y);
                }
            }
        }
    }
}
