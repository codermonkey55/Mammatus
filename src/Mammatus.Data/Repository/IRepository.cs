using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mammatus.Data.Query;

namespace Mammatus.Data.Repository
{
    public interface IRepository<T> where T: class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Copy(T source, T target);
        void Flush();

        T Get(object key);
        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> Table { get; }

        int Count(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate);
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order);
        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count);
    }
}