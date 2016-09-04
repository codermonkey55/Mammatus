using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mammatus.Extensions
{
    public static class ConcurrentExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
                // do nothing
            }
        }

        public static void AddOrReplace<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V value)
        {
            dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }

        public static IEnumerable<T> DequeueAll<T>(this ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
                yield return item;
            }
        }
    }
}
