using System;
using System.Collections.Generic;

namespace Mammatus.Core.IoC
{
    internal static class RepositoryManager
    {
        private static Dictionary<Type, RepositoryType> Repository { get; set; }

        static RepositoryManager()
        {
            Init();
        }

        internal static void Init()
        {
            Repository = new Dictionary<Type, RepositoryType>();
        }

        internal static int GetMappedCount()
        {
            return Repository.Count;
        }

        internal static void Add(Type contract, Type concret, bool overrideExistence, bool canCache, object[] constructorParams)
        {
            Remove(contract);

            lock (Repository)
            {
                if (Repository.ContainsKey(contract))
                {
                    if (overrideExistence)
                        return;

                    Repository.Remove(contract);
                }

                Repository.Add(contract, new RepositoryType(concret, canCache, constructorParams));
            }
        }

        internal static void Add(Type contract, Type concret, object instance, bool overrideExistence, bool canCache, object[] constructorParams)
        {
            Remove(contract);

            lock (Repository)
            {
                if (Repository.ContainsKey(contract))
                {
                    if (overrideExistence)
                        return;

                    Repository.Remove(contract);
                }

                Repository.Add(contract, new RepositoryType(concret, instance, true, constructorParams));
            }
        }

        internal static void Add<T>(Type contract, Func<object> resolver)
        {
            Remove(contract);

            lock (Repository)
            {
                if (Repository.ContainsKey(contract))
                    Repository.Remove(contract);

                Repository.Add(contract, new RepositoryType(contract, resolver));
            }
        }

        internal static void Remove(Type contract)
        {
            lock (Repository)
            {
                if (Repository.ContainsKey(contract))
                    Repository.Remove(contract);
            }
        }

        internal static bool Exists(Type contract)
        {
            lock (Repository)
            {
                return Repository.ContainsKey(contract);
            }
        }

        internal static object Resolve(Type contract, bool canCache, bool forceResolve)
        {
            lock (Repository)
            {
                RepositoryType repository;

                if (!Repository.TryGetValue(contract, out repository))
                {
                    if (forceResolve)
                    {
                        throw new Exception($"Cannot resolve instance of type {contract.FullName}");
                    }

                    return null;
                }

                if (repository.UseResolver)
                    return repository.Resolver();

                if (!canCache || !repository.CanCache)
                    return Activator.CreateInstance(repository.Type);

                if (repository.Instance == null)
                    repository.Instance = Activator.CreateInstance(repository.Type, repository.Params);

                return repository.Instance;
            }
        }

        internal static T Resolve<T>(bool canCache = true, bool forceResolve = true)
        {
            return (T)Resolve(typeof(T), canCache, forceResolve);
        }
    }
}
