using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Mammatus.ServiceModel.Extensibility.Providers
{
    public abstract class InstanceProviderBase : IInstanceProvider
    {
        protected InstanceProviderBase() { }

        public virtual object GetInstance(InstanceContext instanceContext, Message message)
        {
            throw new System.NotImplementedException();
        }

        public virtual object GetInstance(InstanceContext instanceContext)
        {
            throw new NotImplementedException();
        }

        public virtual void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            throw new NotImplementedException();
        }
    }
}
