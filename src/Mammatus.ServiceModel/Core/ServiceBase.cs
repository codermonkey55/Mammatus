using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mammatus.ServiceModel.Helpers;
using Mammatus.ServiceModel.State;

namespace Mammatus.ServiceModel.Core
{
    public abstract class ServiceBase
    {
        private readonly IOperationContextProvider _operationContextProvider;
        protected virtual IOperationContextProvider OperationContextProvider { get { return this._operationContextProvider; } }

        public ServiceInspector Inspector { get; set; }
        public InstanceScopeState InstanceState { get; set; }
        public OperationScopeState OperationState { get; set; }

        public ServiceBase()
        {
            var operactionContextProvider = ServiceModel.State.OperationContextProvider.Create();
            this._operationContextProvider = operactionContextProvider;
            this.Inspector = ServiceInspector.Create();
            this.InstanceState = InstanceScopeState.Create(operactionContextProvider);
            this.OperationState = OperationScopeState.Create(operactionContextProvider);
        }

        public ServiceBase(IOperationContextProvider operactionContextProvider)
        {
            this._operationContextProvider = operactionContextProvider;
            this.Inspector = ServiceInspector.Create();
            this.InstanceState = InstanceScopeState.Create(operactionContextProvider);
            this.OperationState = OperationScopeState.Create(operactionContextProvider);
        }
    }
}
