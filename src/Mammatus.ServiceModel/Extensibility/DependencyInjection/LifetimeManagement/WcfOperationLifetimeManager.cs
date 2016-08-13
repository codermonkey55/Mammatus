using Mammatus.ServiceModel.Runtime.OperationContext;
using Mammatus.ServiceModel.State;

namespace Mammatus.ServiceModel.Extensibility.DependencyInjection.LifetimeManagement
{
    public sealed class WcfOperationLifetimeManager
    {
        private readonly string _key;

        private readonly OperationScopeState _operationScopeState;

        public WcfOperationLifetimeManager()
        {
            _key = System.Guid.NewGuid().ToString();

            _operationScopeState = new OperationScopeState(new OperationContextProvider());
        }

        public object GetValue()
        {
            object result;

            _operationScopeState.TryGet(_key, out result);

            return result;
        }

        public void SetValue(object newValue)
        {
            _operationScopeState.Put(_key, newValue);
        }

        public void RemoveValue()
        {
            _operationScopeState.Remove(_key);
        }
    }
}
