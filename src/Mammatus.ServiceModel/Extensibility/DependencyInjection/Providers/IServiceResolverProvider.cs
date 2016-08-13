using Mammatus.ServiceModel.Extensibility.DependencyInjection.Resolvers;

namespace Mammatus.ServiceModel.Extensibility.DependencyInjection.Providers
{
    public interface IServiceResolverProvider
    {
        IServiceResolver GetResolver();
    }
}