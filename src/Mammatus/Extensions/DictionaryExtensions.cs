using Mammatus.Delegates;
using System;
using System.Collections.Generic;

namespace Mammatus.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Each<T1, T2>(this Dictionary<T1, T2> dictionary, Action<T1, T2> callback)
        {
            foreach (var keyValuePair in dictionary)
            {
                callback(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public static void EachWithIndex<T1, T2>(this Dictionary<T1, T2> dictionary, Action<T1, T2, int> callback)
        {
            var i = 0;
            foreach (var keyValuePair in dictionary)
            {
                callback(keyValuePair.Key, keyValuePair.Value, i++);
            }
        }

        public static void RemoveAll<T1, T2>(this Dictionary<T1, T2> dictionary, Predicate<T1, T2> callback)
        {
            var keysToRemove = new List<T1>();
            foreach (var keyValuePair in dictionary)
            {
                if (callback(keyValuePair.Key, keyValuePair.Value))
                {
                    keysToRemove.Add(keyValuePair.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                dictionary.Remove(key);
            }
        }
    }
}
