using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mammatus.Data.Repository;

namespace Mammatus.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static IRepository<T> DeleteRange<T>(this IRepository<T> repository, IEnumerable<T> deleted)
            where T : class
        {
            if (deleted != null)
            {
                foreach (var item in deleted)
                {
                    repository.Delete(item);
                }
            }
            return repository;
        }

        public static IRepository<T> InsertRange<T>(this IRepository<T> repository, IEnumerable<T> inserted)
            where T : class
        {
            if (inserted != null)
            {
                foreach (var item in inserted)
                {
                    repository.Create(item);
                }
            }
            return repository;
        }
    }
}
