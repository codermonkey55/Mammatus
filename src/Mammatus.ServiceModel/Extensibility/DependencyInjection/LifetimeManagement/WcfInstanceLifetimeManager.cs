using Mammatus.ServiceModel.Runtime.OperationContext;
using Mammatus.ServiceModel.State;

namespace Mammatus.ServiceModel.Extensibility.DependencyInjection.LifetimeManagement
{
    public sealed class WcfInstanceLifetimeManager
    {
        private readonly string _key;

        private readonly InstanceScopeState _instanceScopeState;

        public WcfInstanceLifetimeManager()
        {
            _key = System.Guid.NewGuid().ToString();
            _instanceScopeState = new InstanceScopeState(new OperationContextProvider());
        }

        public object GetValue()
        {
            object result;

            _instanceScopeState.TryGet(_key, out result);

            return result;
        }

        public void SetValue(object newValue)
        {
            _instanceScopeState.Put(_key, newValue);
        }

        public void RemoveValue()
        {
            _instanceScopeState.Remove(_key);
        }
    }
}
