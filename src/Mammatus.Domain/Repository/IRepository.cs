using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mammatus.Core.Data;
using Mammatus.Domain.Core;

namespace Mammatus.Domain.Repository
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        void Add(TEntity item);

        void Attach(TEntity item);

        IEnumerable<TEntity> GetAll();

        TEntity GetById(TKey id);

        IEnumerable<TEntity> GetFiltered(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> orderByExpression = null,
            bool ascending = true);

        PagedElements<TEntity> GetPaged(
            int pageIndex,
            int pageCount,
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> orderByExpression = null,
            bool ascending = true);

        void Modify(TEntity item);

        void Remove(TEntity item);
    }
}