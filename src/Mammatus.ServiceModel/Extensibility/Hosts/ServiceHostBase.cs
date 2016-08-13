using System;
using System.ServiceModel;

namespace Mammatus.ServiceModel.Extensibility.Hosts
{
    public abstract class ServiceHostBase : ServiceHost
    {
        protected ServiceHostBase() { }

        protected ServiceHostBase(System.Type serviceType, Uri[] baseAddresses) : base(serviceType, baseAddresses) { }
    }
}
