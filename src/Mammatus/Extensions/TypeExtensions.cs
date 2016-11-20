using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Extensions
{
    public static class TypeExtensions
    {
        private static bool IsNull<T>(this T obj)
        {
            return obj == null;
        }

        public static T To<T>(this object item)
        {
            T outItem = default(T);
            try
            {
                if (item != null)
                    outItem = (T)Convert.ChangeType(item, typeof(T));
            }
            catch (Exception)
            {
                outItem = default(T);
            }
            return outItem;
        }

        public static T To<TIn, T>(this TIn item, Func<TIn, T> selector)
        {
            return selector.Invoke(item);
        }

        public static T Map<T>(this object item, T target)
        {
            var sourceProps = item.GetType().GetProperties().ToList();

            sourceProps.ForEach(p =>
            {
                var typeTarget = target.GetType();
                if (!typeTarget.GetProperty(p.Name).IsNull())
                {
                    var prop = typeTarget.GetProperty(p.Name);
                    if (prop.CanWrite)
                    {
                        if (prop.GetValue(target, null).IsNull())
                        {
                            var value = p.GetValue(item, null);
                            if (!value.IsNull())
                            {
                                prop.SetValue(target, value, null);
                            }
                        }
                    }
                }
            });

            return target;
        }

        /// <summary>
        /// Returns an array of all concrete subclasses of the provided type.
        /// </summary>
        public static Type[] Subclasses(this Type type)
        {
            var typeList = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().ForEach(a => typeList.AddRange(a.GetTypes()));
            return typeList.Where(t => t.IsSubclassOf(type) && !t.IsAbstract).ToArray();
        }

        /// <summary>
        /// Returns an array of the provided type and all concrete subclasses of that type.
        /// </summary>
        public static Type[] TypeAndSubclasses(this Type type)
        {
            var typeList = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().ForEach(a => typeList.AddRange(a.GetTypes()));
            return typeList.Where(t => (t == type || t.IsSubclassOf(type)) && !t.IsAbstract).ToArray();
        }

        /// <summary>
        /// Determines whether the specified Type is primitive or not.
        /// </summary>
        /// <param name="type">The System.Type to evaluate</param>
        public static bool IsPrimitive(this Type type)
        {
            if (type.IsNullable())
                type = type.GetNullableUnderlyingType();

            return Type.GetTypeCode(type) != TypeCode.Object
                || type.In(
                    typeof(object),
                    typeof(Enum),
                    typeof(Guid),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan));
        }

        /// <summary>
        /// Returns the underlying type argument of the specified nullable type.
        /// </summary>
        /// <returns>The type argument of the nullableType parameter, if the nullableType
        /// parameter is a closed generic nullable type; otherwise, null.</returns>
        public static Type GetNullableUnderlyingType(this Type t)
        {
            return Nullable.GetUnderlyingType(t);
        }

        /// <summary>
        /// Gets a value indicating whether the System.Type is a nullable or not nullable type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type t)
        {
            return t.GetNullableUnderlyingType() != null;
        }

        /// <summary>
        /// Gets a value indicating whether the System.Type represents a nullable or not nullable
        /// type parameter in the definition of a generic type or method.
        /// </summary>
        public static bool IsGenericParameter(this Type t, bool nullable)
        {
            return nullable && t.IsNullable() && t.GetNullableUnderlyingType().IsGenericParameter
                || !nullable && !t.IsNullable() && t.IsGenericParameter;
        }

        /// <summary>
        /// Gets a value indicating whether the System.Type is a nullable or not nullable value type.
        /// </summary>
        public static bool IsValueType(this Type t, bool nullable)
        {
            return nullable && t.IsNullable() && t.GetNullableUnderlyingType().IsValueType
                || !nullable && !t.IsNullable() && t.IsValueType;
        }

        /// <summary>
        /// Determines whether the <paramref name="givenType"/> is assignable to
        /// <paramref name="genericType"/> taking into account generic definitions (e.g., IFoo<int> is assignable to IFoo<>).
        /// Credits: http://tmont.com/blargh/2011/3/determining-if-an-open-generic-type-isassignablefrom-a-type
        /// </summary>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null || genericType == null)
                return false;

            return givenType == genericType
              || givenType.MapsToGenericTypeDefinition(genericType)
              || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
              || givenType.BaseType.IsAssignableToGenericType(genericType);
        }

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return givenType
              .GetInterfaces()
              .Where(it => it.IsGenericType)
              .Any(it => it.GetGenericTypeDefinition() == genericType);
        }

        private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
              && givenType.IsGenericType
              && givenType.GetGenericTypeDefinition() == genericType;
        }
    }
}
