using FluentNHibernate.Mapping;

namespace Mammatus.Data.NHibernate.FluentMappings.BaseMaps
{
    public class CommonMap<TEntity> : ClassMap<TEntity>
    {
        public CommonMap()
        {
            this.DynamicInsert();

            this.DynamicUpdate();

            this.Polymorphism.Explicit();
        }
    }
}
