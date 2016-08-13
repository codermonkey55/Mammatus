using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Mammatus.ServiceModel.Extensibility.Dispatchers;

namespace Mammatus.ServiceModel.Extensibility.Behaviors.Service
{
    public sealed class ExceptionHandlingServiceBehavior : ServiceBehaviorBase
    {
        public override void AddBindingParameters(ServiceDescription serviceDescription,
                                                  ServiceHostBase serviceHostBase,
                                                  Collection<ServiceEndpoint> endpoints,
                                                  BindingParameterCollection bindingParameters)
        {

        }

        public override void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = (ChannelDispatcher)channelDispatcherBase;

                channelDispatcher.ErrorHandlers.Add(new SoapErrorHandlerDispatcher());
            }
        }

        public override void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}
