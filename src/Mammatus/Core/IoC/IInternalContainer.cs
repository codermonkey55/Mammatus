using System.Collections.Generic;

namespace Mammatus.Core.IoC
{
    using System;

    public interface IInternalContainer
    {
        void Register<TService, TImplementation>() where TImplementation : TService;

        void Register<TService, TImplementation>(string named) where TImplementation : TService;

        void Register(Type service, Type implementation) ;

        void Register(Type service, Type implementation, string named);

        void RegisterGeneric(Type service, Type implementation);

        void RegisterGeneric(Type service, Type implementation, string named);

        void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;

        void RegisterSingleton<TService, TImplementation>(string named) where TImplementation : TService;

        void RegisterSingleton(Type service, Type implementation);

        void RegisterSingleton(Type service, Type implementation, string named);

        void RegisterInstance<TService>(TService instance) where TService : class ;

        void RegisterInstance<TService>(TService instance, string named) where TService : class ;

        void RegisterInstance(Type service, object instance);

        void RegisterInstance(Type service, object instance, string named);

        T GetInstance<T>();

        IEnumerable<T> GetAllInstances<T>();
    }
}