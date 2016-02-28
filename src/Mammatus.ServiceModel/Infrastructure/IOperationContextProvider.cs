using System;
using Mammatus.Core.State;

namespace Mammatus.ServiceModel.State
{
    public interface IOperationContextProvider : IContextProvider
    {
        IOperationContextWrapper OperationContext { get; }
    }
}
