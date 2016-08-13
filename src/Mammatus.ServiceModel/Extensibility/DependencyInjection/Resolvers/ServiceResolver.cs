using System;

namespace Mammatus.ServiceModel.Extensibility.DependencyInjection.Resolvers
{
    public class ServiceResolver : IServiceResolver
    {
        public object Resolve(System.Type serviceType)
        {
            return Activator.CreateInstance(serviceType);
        }

        public void Register(System.Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
