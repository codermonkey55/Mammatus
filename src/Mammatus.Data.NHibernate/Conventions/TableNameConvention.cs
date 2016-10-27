using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Mammatus.Data.NHibernate.Conventions
{
    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table("[dbo].[" + instance.EntityType.Name + "s]");
        }
    }
}