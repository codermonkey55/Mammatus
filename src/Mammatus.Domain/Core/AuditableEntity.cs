
namespace Mammatus.Domain.Core
{
    using System;

    [Serializable]
    public abstract class AuditableEntity<TEntity, TKey> : BaseEntity<TEntity, TKey>, IAuditableEntity
        where TEntity : BaseEntity<TEntity, TKey>
        where TKey : struct, IEquatable<TKey>
    {
        public virtual DateTime CreatedAt
        {
            get;
            set;
        }

        public virtual string CreatedBy
        {
            get;
            set;
        }

        public virtual DateTime UpdatedAt
        {
            get;
            set;
        }

        public virtual string UpdatedBy
        {
            get;
            set;
        }

        public virtual DateTime DeletedAt
        {
            get;
            set;
        }

        public virtual string DeletedBy
        {
            get;
            set;
        }
    }
}