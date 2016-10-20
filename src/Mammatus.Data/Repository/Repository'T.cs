using Mammatus.Data.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mammatus.Data.Repository
{
    public abstract class Repository<T> : Repository, IRepository<T> where T : class
    {
        #region Abstract Methods
        public abstract void Create(T entity);

        public abstract void Update(T entity);

        public abstract void Delete(T entity);

        public abstract void Copy(T source, T target);

        public abstract void Flush();

        public abstract T Get(object key);

        public abstract IQueryable<T> Table { get; }

        #endregion

        #region Virtual Methods

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).FirstOrDefault();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).Count();
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate).Any();
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? Table : Table.Where(predicate);
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public virtual IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
                                           int count)
        {
            return Fetch(predicate, order).Skip(skip).Take(count);
        }
        #endregion

        #region IRepository<T> Members

        void IRepository<T>.Create(T entity)
        {
            Create(entity);
        }

        void IRepository<T>.Update(T entity)
        {
            Update(entity);
        }

        void IRepository<T>.Delete(T entity)
        {
            Delete(entity);
        }

        void IRepository<T>.Copy(T source, T target)
        {
            Copy(source, target);
        }

        void IRepository<T>.Flush()
        {
            Flush();
        }

        T IRepository<T>.Get(object key)
        {
            return Get(key);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> predicate)
        {
            return Get(predicate);
        }

        IQueryable<T> IRepository<T>.Table
        {
            get { return Table; }
        }

        int IRepository<T>.Count(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate);
        }

        IQueryable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate)
        {
            return Fetch(predicate);
        }

        IQueryable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            return Fetch(predicate, order);
        }

        IQueryable<T> IRepository<T>.Fetch(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip,
                                            int count)
        {
            return Fetch(predicate, order, skip, count);
        }

        #endregion
    }
}
