using Mammatus.Core.ValueObjects;
using Mammatus.Extensions;
using System;
using System.Reflection;

namespace Mammatus.Core.IoC
{
    public static class Kernel
    {
        public static Map<TContract> Map<TContract>()
            where TContract : class
        {
            if (!(typeof(TContract).GetTypeInfo().IsInterface || typeof(TContract).GetTypeInfo().IsAbstract))
                throw new Exception("O mapeamento deve iniciar por uma interface ou classe abstrata");

            return new Map<TContract>(typeof(TContract));
        }

        public static void Register(Type type)
        {
            RepositoryManager.Add(type, type, true, true, null);
        }

        public static void Register<TContract>(bool canChace)
        {
            RepositoryManager.Add(typeof(TContract), typeof(TContract), false, canChace, null);
        }

        public static void Register<TContract>(TContract instance)
        {
            RepositoryManager.Add(typeof(TContract), typeof(TContract), instance, true, true, null);
        }

        public static void Resolve(object target)
        {
            foreach (AttributeInfo<InjectAttribute> item in target.GetAttributes<InjectAttribute>())
            {
                item.Property.SetValue(target, RepositoryManager.Resolve(item.Property.PropertyType, item.Attribute.CanCache, item.Attribute.ForceResolve), null);
            }
        }

        public static TContract Resolve<TContract>(Type type)
        {
            return (TContract)RepositoryManager.Resolve(type, true, true);
        }

        public static TContract Resolve<TContract>(bool canCache = true)
        {
            return RepositoryManager.Resolve<TContract>(canCache);
        }

        public static bool CanResolve(Type type)
        {
            return RepositoryManager.Exists(type);
        }

        public static bool CanResolve<TContract>()
        {
            return RepositoryManager.Exists(typeof(TContract));
        }

        public static void Reset()
        {
            RepositoryManager.Init();
        }

        public static int GetMappedCount()
        {
            return RepositoryManager.GetMappedCount();
        }
    }
}
