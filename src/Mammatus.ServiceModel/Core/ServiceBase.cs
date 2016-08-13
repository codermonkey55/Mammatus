using System;
using System.ServiceModel;
using Mammatus.ServiceModel.Helpers;
using Mammatus.ServiceModel.Runtime;
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

        protected ServiceBase()
        {
            var operactionContextProvider = Runtime.OperationContext.OperationContextProvider.Create();
            this._operationContextProvider = operactionContextProvider;
            this.Inspector = ServiceInspector.Create();
            this.InstanceState = InstanceScopeState.Create(operactionContextProvider);
            this.OperationState = OperationScopeState.Create(operactionContextProvider);
        }

        protected ServiceBase(IOperationContextProvider operactionContextProvider)
        {
            this._operationContextProvider = operactionContextProvider;
            this.Inspector = ServiceInspector.Create();
            this.InstanceState = InstanceScopeState.Create(operactionContextProvider);
            this.OperationState = OperationScopeState.Create(operactionContextProvider);
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> codetoExecute)
        {
            try
            {
                return codetoExecute.Invoke();
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected void ExecuteFaultHandledOperation(Action codetoExecute)
        {
            try
            {
                codetoExecute.Invoke();
            }
            catch (FaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
