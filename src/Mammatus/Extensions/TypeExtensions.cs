using System;
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

    }
}
