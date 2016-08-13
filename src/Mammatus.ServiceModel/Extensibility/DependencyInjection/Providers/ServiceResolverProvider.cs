using System;
using Mammatus.ServiceModel.Extensibility.DependencyInjection.Resolvers;

namespace Mammatus.ServiceModel.Extensibility.DependencyInjection.Providers
{
    public class ServiceResolverProvider : IServiceResolverProvider
    {
        private static IServiceResolverProvider _serviceResolverProvider;

        public static IServiceResolverProvider Current
        {
            get { return _serviceResolverProvider; }
        }

        private readonly Func<IServiceResolver> _resolverFunc;

        public ServiceResolverProvider(Func<IServiceResolver> resolverFunc)
        {
            _resolverFunc = resolverFunc;
        }

        public IServiceResolver GetResolver()
        {
            return default(IServiceResolver);
        }

        public static void InitProvider(IServiceResolverProvider serviceResolverProvider)
        {
            _serviceResolverProvider = serviceResolverProvider;
        }

        public static void InitProvider(Func<IServiceResolver> resolverFunc)
        {
            _serviceResolverProvider = new ServiceResolverProvider(resolverFunc);
        }
    }
}