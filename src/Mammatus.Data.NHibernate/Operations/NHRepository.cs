﻿using Mammatus.Domain.Core;
using Mammatus.Domain.Repository;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace Mammatus.Data.NHibernate.DbOperations
{
    public class NHRepository<TEntity, TKey> : BaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        //private readonly ILogger logger;
        private readonly ISession session;

        public NHRepository(ISession session) : base()
        {
            //this.logger = LoggerManager.GetLogger(GetType());
            //this.logger.Debug(string.Format(CultureInfo.InvariantCulture, "Created repository for type: {0}", typeof(TEntity).Name));
            this.session = session;
        }

        protected override void InternalAdd(TEntity entity)
        {
            this.session.Save(entity);
        }

        protected override void InternalAttach(TEntity entity)
        {
            this.session.Lock(entity, LockMode.None);
        }

        protected override void InternalModify(TEntity entity)
        {
            if (!this.session.Contains(entity))
            {
                this.session.Update(entity);
            }
        }

        protected override void InternalRemove(TEntity entity)
        {
            this.session.Lock(entity, LockMode.None);
            this.session.Delete(entity);
        }

        protected override IQueryable<TEntity> Query()
        {
            return this.session.Query<TEntity>();
        }

        protected override TEntity Load(TKey id)
        {
            return this.session.Get<TEntity>(id);
        }
    }
}