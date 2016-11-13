using Mammatus.Domain.Core;
using System;

namespace Mammatus.Data.NHibernate.FluentMappings.BaseMaps
{
    public class TenantScopedEntityMap<TEntity, TKey> : AuditableEntityMap<TEntity, TKey>
        where TEntity : TenantScopedEntity<TEntity, TKey>
        where TKey : struct, IEquatable<TKey>
    {
        public TenantScopedEntityMap()
        {
            this.Map(x => x.TenantId)
                .Nullable();
        }
    }

    public class TenantScopedEntityMap<TEntity> : TenantScopedEntityMap<TEntity, Guid>
        where TEntity : TenantScopedEntity<TEntity, Guid>
    {

    }
}