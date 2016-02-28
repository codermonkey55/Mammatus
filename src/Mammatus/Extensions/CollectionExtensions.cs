using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Collection.Extensions
{
    public static class ICollectionExtentions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    source.Add(item);
                }
            }
            return source;
        }

        public static ICollection<T> RemoveRange<T>(this ICollection<T> source, IEnumerable<T> itemsToBeremoved)
        {
            if (itemsToBeremoved != null)
                return source.RemoveRange(itemsToBeremoved.ToArray());
            return source;
        }

        public static ICollection<T> RemoveRange<T>(this ICollection<T> source, params T[] parametersToBeRemoved)
        {
            if (parametersToBeRemoved != null)
            {
                foreach (var parameter in parametersToBeRemoved)
                {
                    source.Remove(parameter);
                }
            }
            return source;
        }
    }
}
