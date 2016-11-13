using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Mammatus.Data.NHibernate.Conventions
{
    public class ForeignKeyConstraintNames : IReferenceConvention, IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            string entity = instance.EntityType.Name;
            string member = instance.Member.Name;
            string child = instance.ChildType.Name;

            instance.Key.ForeignKey($"FK_{entity}{member}_{child}");
        }

        public void Apply(IManyToOneInstance instance)
        {
            string entity = instance.EntityType.Name;
            string member = instance.Property.Name;

            instance.ForeignKey($"FK_{entity}_{member}");
        }
    }
}