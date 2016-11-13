using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace Mammatus.Data.NHibernate.Filters
{
    public class TenantFilter : FilterDefinition
    {
        public TenantFilter()
        {
            WithName("TenantFilter");
            WithCondition("TenantId = :tenantId");

            AddParameter("tenantId", new GuidType());
        }
    }
}
