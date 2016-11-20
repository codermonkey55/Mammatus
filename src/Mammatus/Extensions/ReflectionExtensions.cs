
using Mammatus.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mammatus.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Determines whether the value is the name of one of the given properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static bool IsProperty<T>(this string name, params Expression<Func<T>>[] properties)
        {
            foreach (var property in properties)
            {
                if (name.IsProperty(property))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the value is the name of the given property.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="name">The tested string</param>
        /// <param name="propertyExpression">The property</param>
        /// <returns>True of name is equals to the name of the property's name</returns>
        public static bool IsProperty<T>(this string name, Expression<Func<T>> propertyExpression)
        {
            var body = propertyExpression.Body as MemberExpression;
            var info = body.Member as PropertyInfo;
            return name == info.Name;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(this object owner, Expression<Func<T>> propertyExpression)
        {
            var body = propertyExpression.Body as MemberExpression;
            var info = body.Member as PropertyInfo;
            return info.Name;
        }

        public static Type[] ToTypeArray(this ParameterInfo[] parameters)
        {
            if (parameters.Length == 0)
                return Type.EmptyTypes;
            var types = new Type[parameters.Length];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameters[i].ParameterType;
            }
            return types;
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            var properties = type.GetTypeInfo().DeclaredProperties.ToList();
            var baseType = type.GetTypeInfo().BaseType;

            if (baseType != typeof(object))
            {
                var baseProperties = GetProperties(baseType);
                properties.AddRange(baseProperties);
            }

            return properties;
        }

        public static List<AttributeInfo<TAttribute>> GetAttributes<TAttribute>(this object objeto)
            where TAttribute : Attribute
        {
            var propriedades = objeto.GetType().GetProperties();

            var query = from p in propriedades
                        let attr = p.GetCustomAttributes(typeof(TAttribute), true)
                        where attr.Count() == 1
                        select new AttributeInfo<TAttribute>
                        {
                            Property = p,
                            Attribute = (TAttribute)attr.FirstOrDefault()
                        };

            return query.ToList();
        }
    }
}
