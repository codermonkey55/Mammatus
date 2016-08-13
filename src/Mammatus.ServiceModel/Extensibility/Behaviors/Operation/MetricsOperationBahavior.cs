using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Mammatus.ServiceModel.Extensibility.Inspectors;

namespace Mammatus.ServiceModel.Extensibility.Behaviors.Operation
{
    public sealed class MetricsOperationBahavior : OperationBehaviorBase
    {
        public override void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters) { }

        public override void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.ParameterInspectors.Add(new MetricsParameterInspector());
        }

        public override void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(new MetricsParameterInspector());
        }

        public override void Validate(OperationDescription operationDescription) { }
    }
}
