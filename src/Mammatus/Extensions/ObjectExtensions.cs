using Mammatus.Library.Generic;
using Mammatus.Library.Reflection.Emitter;
using System;

namespace Mammatus.Extensions
{
    public static class ObjectExtensions
    {
        public static T IfNull<T>(this T obj, Action action)
        {
            if (obj == null)
            {
                action();
            }

            return obj;
        }

        public static T IfNull<T>(this T obj, Func<T> func)
        {
            if (obj == null)
            {
                return func();
            }

            return obj;
        }

        public static T IfNotNull<T>(this T obj, Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }

            return obj;
        }

        public static T IfNotNull<T>(this T obj, Action action)
        {
            if (obj != null)
            {
                action();
            }

            return obj;
        }

        public static Option<T> AsOption<T>(this T obj)
        {
            return Option<T>.Create(obj);
        }

        public static Type GetTypeAdjusted(this object obj)
        {
            var wrapper = obj as ValueTypeHolder;
            return wrapper == null
                ? obj is Type ? obj as Type : obj.GetType()
                : wrapper.Value.GetType();
        }

        public static Type[] ToTypeArray(this object[] objects)
        {
            if (objects.Length == 0)
                return Type.EmptyTypes;
            var types = new Type[objects.Length];
            for (int i = 0; i < types.Length; i++)
            {
                var obj = objects[i];
                types[i] = obj != null ? obj.GetType() : null;
            }
            return types;
        }
    }
}
