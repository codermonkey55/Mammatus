using System.Collections.Generic;
using System.Linq;
using Mammatus.Core.IoC;
using Mammatus.Exceptions;

namespace Mammatus.ComponentModel.Containers
{
    using System;

    public class InternalContainer : IInternalContainer
    {
        public static InternalContainer Current
        {
            get
            {
                return new InternalContainer();
            }
        }

        private Action<Type, object> registerInstanceCallback;
        private Action<Type, Type> registerTypeCallback;
        private Func<Type, IEnumerable<object>> resolveAllCallback;
        private Func<Type, object> resolveCallback;

        public TDependency[] GetAllInstances<TDependency>()
        {
            if (resolveAllCallback != null)
            {
                IEnumerable<object> services = resolveAllCallback(typeof(TDependency));

                if (services != null)
                {
                    return services.Cast<TDependency>().ToArray();
                }
            }

            return new List<TDependency>().ToArray();
        }


        IEnumerable<TDependency> IInternalContainer.GetAllInstances<TDependency>()
        {
            return this.GetAllInstances<TDependency>();
        }

        public TDependency GetInstance<TDependency>()
        {
            return (TDependency)GetInstance(typeof(TDependency));
        }

        public object GetInstance(Type dependencyType)
        {
            return resolveCallback(dependencyType);
        }

        public void Initialize(
            Action<Type, Type> registerType,
            Action<Type, object> registerInstance,
            Func<Type, object> resolve,
            Func<Type, IEnumerable<object>> resolveAll)
        {
            registerTypeCallback = registerType;
            registerInstanceCallback = registerInstance;
            resolveCallback = resolve;
            resolveAllCallback = resolveAll;
        }

        public void RegisterInstance<I>(object instance)
        {
            if (registerInstanceCallback != null)
            {
                registerInstanceCallback(typeof(I), instance);
            }
        }

        public void RegisterInstance(Type @type, object instance)
        {
            if (registerInstanceCallback != null)
            {
                registerInstanceCallback(@type, instance);
            }
        }

        public void RegisterType<I, T>()
        where T : I
        {
            if (registerTypeCallback != null)
            {
                registerTypeCallback(typeof(I), typeof(T));
            }
        }

        public void RegisterType(Type @interface, Type @type)
        {
            if (registerTypeCallback != null)
            {
                registerTypeCallback(@interface, @type);
            }
        }

        public TDependency TryGetInstance<TDependency>()
        {
            try
            {
                IEnumerable<TDependency> services = GetAllInstances<TDependency>();

                if (services != null)
                {
                    return (TDependency)services.FirstOrDefault();
                }
            }
            catch (NullReferenceException)
            {
                throw new DependencyException("ServiceLocator has not been initialized; " +
                                            "I was trying to retrieve " + typeof(TDependency));
            }

            return default(TDependency);
        }

        public void Register<TService, TImplementation>() where TImplementation : TService
        {
            throw new NotImplementedException();
        }

        public void Register<TService, TImplementation>(string named) where TImplementation : TService
        {
            throw new NotImplementedException();
        }

        public void Register(Type service, Type implementation)
        {
            throw new NotImplementedException();
        }

        public void Register(Type service, Type implementation, string named)
        {
            throw new NotImplementedException();
        }

        public void RegisterGeneric(Type service, Type implementation)
        {
            throw new NotImplementedException();
        }

        public void RegisterGeneric(Type service, Type implementation, string named)
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton<TService, TImplementation>(string named) where TImplementation : TService
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type service, Type implementation)
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type service, Type implementation, string named)
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance<TService>(TService instance, string named) where TService : class
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance(Type service, object instance, string named)
        {
            throw new NotImplementedException();
        }

    }
}