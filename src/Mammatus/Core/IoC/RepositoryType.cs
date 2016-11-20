using System;

namespace Mammatus.Core.IoC
{
    internal class RepositoryType
    {
        public RepositoryType(Type type)
            : this(type, null, true, null)
        {

        }

        public RepositoryType(Type type, object[] constructorParams)
            : this(type, null, true, constructorParams)
        {
        }

        public RepositoryType(Type type, bool canCache, object[] constructorParams)
            : this(type, null, canCache, constructorParams)
        {
        }

        public RepositoryType(Type type, Func<object> resolver)
        {
            Type = type;
            Instance = null;
            CanCache = false;
            Resolver = resolver;
            UseResolver = true;
        }

        public RepositoryType(Type type, object instance, bool canCache, object[] constructorParams)
        {
            Type = type;
            Instance = instance;
            CanCache = canCache;
            UseResolver = false;
            Params = constructorParams;
        }

        internal Func<object> Resolver { get; set; }

        internal bool UseResolver { get; set; }

        internal bool CanCache { get; set; }

        internal Type Type { get; set; }

        internal object Instance { get; set; }

        internal object[] Params { get; set; }
    }
}
