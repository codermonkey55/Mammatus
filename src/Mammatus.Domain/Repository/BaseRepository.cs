using Mammatus.Core.Data;
using Mammatus.Domain.Core;
using Mammatus.Extensions;
using Mammatus.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mammatus.Domain.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        //private readonly ILogger logger;

        protected BaseRepository()
        {
            //this.logger = LoggerManager.GetLogger(GetType());

            //this.logger.Debug(string.Format(CultureInfo.InvariantCulture, "Created repository for type: {0}", typeof(TEntity).Name));
        }

        //protected ILogger Logger
        //{
        //    get
        //    {
        //        return this.logger;
        //    }
        //}

        public virtual void Add(TEntity entity)
        {
            Guard.IsNotNull(entity, "entity");

            this.InternalAdd(entity);

            //this.logger.Debug(string.Format(CultureInfo.InvariantCulture, "Added a {0} entity", typeof(TEntity).Name));
        }

        public void Attach(TEntity entity)
        {
            Guard.IsNotNull(entity, "entity");

            this.InternalAttach(entity);

            //this.logger.Debug(string.Format(CultureInfo.InvariantCulture, "Attached {0} to context", typeof(TEntity).Name));
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.Query().ToList();
        }

        public TEntity GetById(TKey id)
        {
            return this.Load(id);
        }

        public IEnumerable<TEntity> GetFiltered(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> orderByExpression = null,
            bool ascending = true)
        {
            // Checking query arguments
            Guard.IsNotNull(filter, "filter");

            //this.logger.Debug(string.Format(CultureInfo.InvariantCulture, "Getting filtered elements {0} with filter: {1}", typeof(TEntity).Name, filter.ToString()));

            var query = this.Query().Where(filter);

            if (orderByExpression == null)
            {
                return query.ToList();
            }

            return ascending
                   ? query.OrderBy(orderByExpression).ToList()
                   : query.OrderByDescending(orderByExpression).ToList();
        }

        public PagedElements<TEntity> GetPaged(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> orderByExpression,
            bool ascending = true)
        {
            // checking arguments for this query
            Guard.Against<ArgumentException>(pageIndex < 0, "pageIndex");
            Guard.Against<ArgumentException>(pageSize <= 0, "pageSize");
            Guard.IsNotNull(orderByExpression, "orderByExpression");
            Guard.IsNotNull(filter, "filter");

            //this.logger.Debug(
            //    string.Format(
            //        CultureInfo.InvariantCulture,
            //        "Getting paged elements {0}, pageIndex: {1}, pageSize {2}, oderBy {3}",
            //        typeof(TEntity).Name,
            //        pageIndex,
            //        pageSize,
            //        orderByExpression.ToString()));

            var query = this.Query().Where(filter);

            int total = query.Count();

            return ascending
                   ? new PagedElements<TEntity>(
                       query.OrderBy(orderByExpression)
                       .Page(pageIndex, pageSize)
                       .ToList(),
                       total)
                   : new PagedElements<TEntity>(
                       query.OrderByDescending(orderByExpression)
                       .Page(pageIndex, pageSize)
                       .ToList(),
                       total);
        }

        public virtual void Modify(TEntity entity)
        {
            Guard.IsNotNull(entity, "entity");

            this.InternalModify(entity);

            //this.logger.Info(string.Format(CultureInfo.InvariantCulture, "Applied changes to: {0}", typeof(TEntity).Name));
        }

        public virtual void Remove(TEntity entity)
        {
            // check entity
            Guard.IsNotNull(entity, "entity");

            this.InternalRemove(entity);

            //this.logger.Debug(string.Format(CultureInfo.InvariantCulture, "Deleted a {0} entity", typeof(TEntity).Name));
        }

        protected abstract void InternalAdd(TEntity entity);

        protected abstract void InternalAttach(TEntity entity);

        protected abstract void InternalModify(TEntity entity);

        protected abstract void InternalRemove(TEntity entity);

        protected abstract IQueryable<TEntity> Query();

        protected abstract TEntity Load(TKey id);

    }
}