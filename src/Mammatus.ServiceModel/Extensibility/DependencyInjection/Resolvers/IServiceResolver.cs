namespace Mammatus.ServiceModel.Extensibility.DependencyInjection.Resolvers
{
    public interface IServiceResolver
    {
        object Resolve(System.Type serviceType);

        void Register(System.Type serviceType);
    }
}
