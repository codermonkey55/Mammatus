using Mammatus.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mammatus.Data.NHibernate.Database
{
    public interface INHGenericRepository<T> : IRepository<T>, INHRepository where T : class
    {

    }

    public abstract class NHGenericRepository<T> : NHRepository, INHGenericRepository<T> where T : class
    {
        protected INHEntityCollection<T> _collection;

        public NHGenericRepository(INHEntityCollection<T> entityCollection) : base(entityCollection)
        {
            _collection = entityCollection;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _collection.Entities.List<T>();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _collection.Linq.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Add(T entity)
        {
            return _collection.Save(entity);
        }

        public T GetById(object id)
        {
            return _collection.Get(id);
        }

        public virtual T Delete(T entity)
        {
            return _collection.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _collection.Update(entity);
        }

        public override void SetCollection(IEntityCollection entityCollection)
        {
            try
            {
                var stronglyTypedCollection = entityCollection as INHEntityCollection<T>;
                if (stronglyTypedCollection != null)
                    _collection = stronglyTypedCollection;
            }
            catch { }

            base.SetCollection(entityCollection);
        }
    }
}
