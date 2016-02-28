using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Mammatus.ServiceModel.ErrorHandling
{
    using System;

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class ServiceFaultContractBehavior : Attribute, IContractBehavior
    {
        private readonly Type[] knownFaultTypes;

        public ServiceFaultContractBehavior()
        {
        }

        public ServiceFaultContractBehavior(Type[] knownFaultTypes)
        {
            this.knownFaultTypes = knownFaultTypes;
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            foreach (var op in contractDescription.Operations)
            {
                foreach (var knownFaultType in knownFaultTypes)
                {
                    if (!op.Faults.Any(f => f.DetailType == knownFaultType))
                    {
                        op.Faults.Add(new FaultDescription(knownFaultType.Name)
                        {
                            DetailType = knownFaultType, Name = knownFaultType.Name
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
            var badType = knownFaultTypes.FirstOrDefault(t => !t.IsDefined(typeof(DataContractAttribute), true));
            if (badType != null)
            {
                throw new ArgumentException(string.Format("The specified fault '{0}' is no data contract. Did you forget to decorate the class with the DataContractAttirbute attribute?", badType));
            }
        }
    }
}