using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Validation
{
    public sealed class CheckIf
    {
        private CheckIf()
        {

        }

        public static void TypeInheritsFrom<TBase>(object instance, string message) where TBase : System.Type
        {
            TypeInheritsFrom<TBase>(instance.GetType(), message);
        }

        public static void TypeInheritsFrom<TBase>(System.Type type, string message)
        {
            if (type.BaseType != typeof(TBase))
                throw new InvalidOperationException(message);
        }

        public static void TypeImplements<TInterface>(object instance, string message)
        {
            TypeImplements<TInterface>(instance.GetType(), message);
        }

        public static void TypeImplements<TInterface>(System.Type type, string message)
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
                throw new InvalidOperationException(message);
        }

        public static void TypeOf<TType>(object instance, string message)
        {
            if (!(instance is TType))
                throw new InvalidOperationException(message);
        }

        public static void AreEqual<TException>(object compare, object instance, string message) where TException : Exception
        {
            if (compare != instance)
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }
    }
}
