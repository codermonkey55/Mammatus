using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mammatus.Data.NHibernate.Conventions
{
    public class StringLengthConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance target)
        {
            var attribute =
                Attribute.GetCustomAttribute(target.Property.MemberInfo, typeof(StringLengthAttribute)) as
                StringLengthAttribute;

            if (attribute != null)
            {
                target.Length(attribute.MaximumLength);
            }
        }
    }
}