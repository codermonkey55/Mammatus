using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Mammatus.ServiceModel.Extensibility.Behaviors.Operation;

namespace Mammatus.ServiceModel.Extensibility.Behaviors.Service
{
    public sealed class MetricsServiceBehavior : ServiceBehaviorBase
    {
        public override void AddBindingParameters(ServiceDescription serviceDescription,
                                                  ServiceHostBase serviceHostBase,
                                                  Collection<ServiceEndpoint> endpoints,
                                                  BindingParameterCollection bindingParameters)
        {

        }

        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var serviceEndpoint = serviceHostBase.Description.Endpoints.FirstOrDefault();

            if (serviceEndpoint != null)
                foreach (OperationDescription od in serviceEndpoint.Contract.Operations)
                    od.Behaviors.Add(new MetricsOperationBahavior());
        }

        public override void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}
