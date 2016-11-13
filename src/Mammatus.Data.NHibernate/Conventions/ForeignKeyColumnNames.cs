using FluentNHibernate;
using FluentNHibernate.Conventions;
using System;

namespace Mammatus.Data.NHibernate.Conventions
{
    public class ForeignKeyColumnNames : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            // many-to-many, one-to-many, join
            if (property == null)
            {
                return type.Name + "Id";
            }

            return property.Name + "Id";
        }
    }
}