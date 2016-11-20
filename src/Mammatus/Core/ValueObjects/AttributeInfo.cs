using System;
using System.Reflection;

namespace Mammatus.Core.ValueObjects
{
    public class AttributeInfo<T>
        where T : Attribute
    {
        public PropertyInfo Property { get; set; }

        public T Attribute { get; set; }
    }
}
