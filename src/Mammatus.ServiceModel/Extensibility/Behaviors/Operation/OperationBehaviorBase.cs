using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Mammatus.ServiceModel.Extensibility.Behaviors.Operation
{
    public abstract class OperationBehaviorBase : Attribute, IOperationBehavior
    {
        public virtual void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public virtual void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            throw new NotImplementedException();
        }

        public virtual void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            throw new NotImplementedException();
        }

        public virtual void Validate(OperationDescription operationDescription)
        {
            throw new NotImplementedException();
        }
    }
}
