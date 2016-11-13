using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Mammatus.Data.NHibernate.Filters;
using Mammatus.Domain.Core;
using Mammatus.Utils;

namespace Mammatus.Data.NHibernate.Conventions
{
    public class ManyToManyTableName : ManyToManyTableNameConvention
    {
        protected override string GetBiDirectionalTableName(
            IManyToManyCollectionInspector collection,
            IManyToManyCollectionInspector otherSide)
        {
            return Inflector.Underscore(collection.EntityType.Name + "_" + otherSide.EntityType.Name).ToUpper();
        }

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
        {
            return Inflector.Underscore(collection.EntityType.Name + "_" + collection.ChildType.Name).ToUpper();
        }
    }

    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            //instance.Table("[dbo].[" + instance.EntityType.Name + "s]");

            instance.Table("`" + Inflector.Underscore(instance.EntityType.Name).ToUpper() + "´");
        }
    }

    public class TenantScopedEntityConvention : IClassConvention, IClassConventionAcceptance
    {
        public void Apply(IClassInstance instance)
        {
            instance.ApplyFilter<TenantFilter>();
        }

        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            criteria.Expect(x => typeof(ITenantScopedEntity).IsAssignableFrom(x.EntityType) && !x.EntityType.IsAbstract);
        }
    }
}