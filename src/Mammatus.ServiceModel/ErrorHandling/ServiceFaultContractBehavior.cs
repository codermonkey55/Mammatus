using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Mammatus.ServiceModel.ErrorHandling
{
    using System;

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class ServiceFaultContractBehavior : Attribute, IContractBehavior
    {
        private readonly Type[] _knownFaultTypes;

        public ServiceFaultContractBehavior()
        {
        }

        public ServiceFaultContractBehavior(Type[] knownFaultTypes)
        {
            this._knownFaultTypes = knownFaultTypes;
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            foreach (var op in contractDescription.Operations)
            {
                foreach (var knownFaultType in _knownFaultTypes)
                {
                    if (op.Faults.All(f => f.DetailType != knownFaultType))
                    {
                        op.Faults.Add(new FaultDescription(knownFaultType.Name)
                        {
                            DetailType = knownFaultType,
                            Name = knownFaultType.Name
                        });
                    }
                }
            }
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // No behavior added.
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            // No dispatch behavior added.
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            var badType = _knownFaultTypes.FirstOrDefault(t => !t.IsDefined(typeof(DataContractAttribute), true));

            if (badType != null)
            {
                throw new ArgumentException(string.Format("The specified fault '{0}' is no data contract. Did you forget to decorate the class with the DataContractAttirbute attribute?", badType));
            }
        }
    }
}