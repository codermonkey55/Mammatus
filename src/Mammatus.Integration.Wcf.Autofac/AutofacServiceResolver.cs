using Autofac;
using Mammatus.ServiceModel.Extensibility.DependencyInjection.Resolvers;
using System;

namespace Mammatus.Integration.Wcf.Autofac
{
    public class AutofacServiceResolver : IServiceResolver
    {
        private readonly IContainer _container;

        public AutofacServiceResolver(IContainer lifetimeScope)
        {
            _container = lifetimeScope;
        }

        public object GetInstance(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public object GetInstance(Type serviceType, string registrationKey)
        {
            return _container.ResolveKeyed(registrationKey, serviceType);
        }

        public TService GetInstance<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }

        public TService GetInstance<TService>(string registrationKey) where TService : class
        {
            return _container.ResolveKeyed<TService>(registrationKey);
        }

        public void Register(Type serviceType)
        {
            var isRegistered = _container.IsRegistered(serviceType);

            if (isRegistered) return;

            //-> Create new "ContainerBuilder" instance.
            var containerBuilder = new ContainerBuilder();

            //-> Declaring new registrations.
            containerBuilder.RegisterType(serviceType);

            //-> Add the registrations to the existing container.
            containerBuilder.Update(_container);
        }

        public void Register<TService>()
        {
            Register(typeof(TService));
        }



        object IServiceResolver.Resolve(Type serviceType)
        {
            return GetInstance(serviceType);
        }

        void IServiceResolver.Register(Type serviceType)
        {
            var isRegistered = _container.IsRegistered(serviceType);

            if (isRegistered) return;

            //-> Create new "ContainerBuilder" instance.
            var containerBuilder = new ContainerBuilder();

            //-> Declaring new registrations.
            containerBuilder.RegisterType(serviceType);

            //-> Add the registrations to the existing container.
            containerBuilder.Update(_container);
        }
    }
}
