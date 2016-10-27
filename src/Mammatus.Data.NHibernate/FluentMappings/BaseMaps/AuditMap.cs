using Mammatus.Data.NHibernate.Entities.Base;

namespace Mammatus.Data.NHibernate.FluentMappings.BaseMaps
{
    public class AuditMap<TEntity> : CommonMap<TEntity> where TEntity : AuditableEntity
    {
        public AuditMap()
        {
            this.Map(x => x.CreateDate);

            this.Map(x => x.EditDate);

            this.Component(x => x.AuditInfo);
        }
    }
}
