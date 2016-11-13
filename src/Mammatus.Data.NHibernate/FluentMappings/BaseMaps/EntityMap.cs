using Mammatus.Data.NHibernate.UserTypes;
using Mammatus.Domain.Core;
using System;

namespace Mammatus.Data.NHibernate.FluentMappings.BaseMaps
{
    public class EntityMap<TEntity, TKey> : BaseClassMap<TEntity>
        where TEntity : BaseEntity<TEntity, TKey>
        where TKey : struct, IEquatable<TKey>
    {
        public EntityMap()
        {
            if (typeof(TKey).Equals(typeof(int)))
            {
                this.Id(x => x.Id)
                    .UnsavedValue(0)
                    .GeneratedBy.Native();
            }

            if (typeof(TKey).Equals(typeof(Guid)))
            {
                this.Id(x => x.Id)
                    .GeneratedBy.GuidComb();
            }

            // Use versioned timestamp as optimistick lock mechanism.
            this.OptimisticLock.Version();

            // Create Insert statements dynamically.
            this.DynamicInsert();

            // Create Update statements dynamically.
            this.DynamicUpdate();

            this.Version(x => x.Version)
                .CustomType<TicksAsString>();
        }
    }
}