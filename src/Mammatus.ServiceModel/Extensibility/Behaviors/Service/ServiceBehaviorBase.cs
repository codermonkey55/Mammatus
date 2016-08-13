using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Mammatus.ServiceModel.Extensibility.Behaviors.Service
{
    public abstract class ServiceBehaviorBase : IServiceBehavior
    {
        public virtual void AddBindingParameters(ServiceDescription serviceDescription,
                                                 ServiceHostBase serviceHostBase,
                                                 Collection<ServiceEndpoint> endpoints,
                                                 BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public virtual void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            throw new NotImplementedException();
        }

        public virtual void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            throw new NotImplementedException();
        }
    }
}
