using Mammatus.Library.Reflection.Emitter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Mammatus.Library.Reflection.Common
{
    [DebuggerStepThrough]
    internal static class Utils
    {
        public static Type GetTypeAdjusted(this object obj)
        {
            var wrapper = obj as ValueTypeHolder;
            return wrapper == null
                ? obj is Type ? obj as Type : obj.GetType()
                : wrapper.Value.GetType();
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

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }
    }
}