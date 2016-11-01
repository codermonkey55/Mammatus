using Mammatus.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            Guard.Against<ArgumentException>(pageIndex < 0, "pageIndex");

            return query.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> query, int pageIndex, int pageSize)
        {
            Guard.Against<ArgumentException>(pageIndex < 0, "pageIndex");

            return query.Skip(pageIndex * pageSize).Take(pageSize);
        }
    }
}