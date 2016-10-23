using Mammatus.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mammatus.Data.Entity.Core
{
    public interface IEFGenericRepository<T> : IRepository<T>, IEFRepository where T : class
    {

    }

    public abstract class EFGenericRepository<T> : EFRepository, IEFGenericRepository<T> where T : class
    {
        protected DbSet<T> _dbset;

        protected IEFEntityCollection<T> _entityCollection;

        public EFGenericRepository(IEFEntityCollection<T> entityCollection) : base(entityCollection)
        {
            _dbset = entityCollection.Entities;
            _entityCollection = entityCollection;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity).Entity;
        }

        public T GetById(object id)
        {
            return _entityCollection.Find(id);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity).Entity;
        }

        public virtual void Edit(T entity)
        {
            base.Modify(entity);
        }

        public override void SetCollection(IEntityCollection entityCollection)
        {
            try
            {
                var stronglyTypedCollection = entityCollection as IEFEntityCollection<T>;
                if (stronglyTypedCollection != null)
                {
                    _dbset = stronglyTypedCollection.Entities;
                    _entityCollection = stronglyTypedCollection;
                }
                else
                    _dbset = ((IEFEntityCollection)entityCollection).Collection<T>();
            }
            catch { }

            base.SetCollection(entityCollection);
        }
    }
}
