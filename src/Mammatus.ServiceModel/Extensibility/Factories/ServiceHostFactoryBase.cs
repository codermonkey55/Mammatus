using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;

namespace Mammatus.ServiceModel.Extensibility.Factories
{
    public abstract class ServiceHostFactoryBase : ServiceHostFactory
    {
        protected ServiceHostFactoryBase() { }

        protected ServiceHost AddOperationBehavior<TOperationBehavior>(ServiceHost host)
            where TOperationBehavior : IOperationBehavior, new()
        {
            var serviceEndpoint = host.Description.Endpoints.FirstOrDefault();

            if (serviceEndpoint != null)
                foreach (OperationDescription od in serviceEndpoint.Contract.Operations)
                    od.Behaviors.Add(new TOperationBehavior());

            return host;
        }

        protected ServiceHost AddOperationBehavior<TOperationBehavior>(ServiceHost host, Func<TOperationBehavior> behaviorProvider)
            where TOperationBehavior : IOperationBehavior
        {
            var serviceEndpoint = host.Description.Endpoints.FirstOrDefault();

            if (serviceEndpoint != null)
                foreach (OperationDescription od in serviceEndpoint.Contract.Operations)
                    od.Behaviors.Add(behaviorProvider.Invoke());

            return host;
        }

        protected TServiceHost AddOperationBehavior<TServiceHost, TOperationBehavior>(TServiceHost host)
            where TServiceHost : ServiceHost
            where TOperationBehavior : IOperationBehavior, new()
        {
            return AddOperationBehavior<TOperationBehavior>(host) as TServiceHost;
        }

        protected TServiceHost AddOperationBehavior<TServiceHost, TOperationBehavior>(TServiceHost host, Func<TOperationBehavior> behaviorProvider)
            where TServiceHost : ServiceHost
            where TOperationBehavior : IOperationBehavior
        {
            return AddOperationBehavior<TOperationBehavior>(host, behaviorProvider) as TServiceHost;
        }
    }
}
