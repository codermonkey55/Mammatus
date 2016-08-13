using System;
using System.ServiceModel.Dispatcher;

namespace Mammatus.ServiceModel.Extensibility.Inspectors
{
    public abstract class ParameterInspectorBase : IParameterInspector
    {
        public virtual void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            throw new NotImplementedException();
        }

        public virtual object BeforeCall(string operationName, object[] inputs)
        {
            throw new NotImplementedException();
        }
    }
}
