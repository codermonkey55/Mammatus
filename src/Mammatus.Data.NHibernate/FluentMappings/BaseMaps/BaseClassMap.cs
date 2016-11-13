using FluentNHibernate.Mapping;

namespace Mammatus.Data.NHibernate.FluentMappings.BaseMaps
{
    public class BaseClassMap<TEntity> : ClassMap<TEntity>
    {
        public BaseClassMap()
        {
            this.DynamicInsert();

            this.DynamicUpdate();

            this.Polymorphism.Explicit();
        }
    }
}
