using System;

namespace Mammatus.Domain.Core
{
    public interface ITenantScopedEntity
    {
        Guid? TenantId { get; }

        void SetTenantId(Guid? tenantId);
    }
}
