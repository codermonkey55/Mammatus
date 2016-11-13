using FluentNHibernate.Mapping;
using Mammatus.Data.NHibernate.Components;

namespace Mammatus.Data.NHibernate.FluentMappings.ComponentMaps
{
    public class AuditableEntityComponentMap : ComponentMap<AuditInfo>
    {
        public AuditableEntityComponentMap()
        {
            this.Map(x => x.CreateDate);

            this.Map(x => x.EditDate);
        }
    }
}
