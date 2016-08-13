using Mammatus.Core.State;
using Mammatus.ServiceModel.Runtime.OperationContext;

namespace Mammatus.ServiceModel.Runtime
{
    public interface IOperationContextProvider : IContextProvider
    {
        IOperationContextWrapper OperationContext { get; }
    }
}
