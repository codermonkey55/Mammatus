using System;
using System.ServiceModel.Dispatcher;

namespace Mammatus.ServiceModel.Extensibility.Dispatchers
{
    public abstract class ErrorHandlerDispatcherBase : IErrorHandler
    {
        protected ErrorHandlerDispatcherBase() { }

        public virtual bool HandleError(Exception error)
        {
            throw new NotImplementedException();
        }

        public virtual void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            throw new NotImplementedException();
        }
    }
}
