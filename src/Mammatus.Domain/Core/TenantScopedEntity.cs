using System;

namespace Mammatus.Domain.Core
{
    [Serializable]
    public abstract class TenantScopedEntity<TEntity, TKey> : AuditableEntity<TEntity, TKey>, IAuditableEntity, ITenantScopedEntity
        where TEntity : AuditableEntity<TEntity, TKey>
        where TKey : struct, IEquatable<TKey>
    {
        public virtual Guid? TenantId
        {
            get;
            protected set;
        }

        public virtual void SetTenantId(Guid? tenantId)
        {
            this.TenantId = tenantId;
        }
    }

    [Serializable]
    public abstract class TenantScopedEntity<TEntity> : TenantScopedEntity<TEntity, Guid>
        where TEntity : AuditableEntity<TEntity, Guid>
    {

    }
}