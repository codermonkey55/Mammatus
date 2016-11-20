using System;
using System.Reflection;

namespace Mammatus.Core.IoC
{
    public class Map<TContract>
    {
        internal Type Contract { get; set; }

        internal Map(Type type)
        {
            Contract = type;
        }

        public void To<TConcret>()
        {
            To<TConcret>(true);
        }

        public void To<TConcret>(params object[] args)
        {
            To<TConcret>(true, args);
        }

        public void To<TConcret>(bool canCache, params object[] args)
        {
            if (typeof(TConcret).GetTypeInfo().IsInterface || typeof(TConcret).GetTypeInfo().IsAbstract)
                throw new Exception("Invalid type, cannot construct instance of type interface and/or abstract.");

            RepositoryManager.Add(Contract, typeof(TConcret), true, canCache, args);
        }

        public void To<TConcret>(TConcret instance, params object[] args)
        {
            RepositoryManager.Add(Contract, typeof(TConcret), instance, true, true, args);
        }

        public void ToFunc(Func<object> resolver)
        {
            RepositoryManager.Add<TContract>(Contract, resolver);
        }
    }
}
