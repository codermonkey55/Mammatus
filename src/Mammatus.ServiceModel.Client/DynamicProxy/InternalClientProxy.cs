using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    /// <summary>
    /// This represents the client proxy as used internally. This proxy is 
    /// recreated when the connection with the service is somehow severed.
    /// </summary>
    /// <typeparam name="TInterface">The interface of the service for which 
    /// we will act as a proxy</typeparam>
    internal class InternalClientProxy<TInterface> :
        ClientBase<TInterface> where TInterface : class
    {
        /// <summary>
        /// 
        /// </summary>
        public InternalClientProxy()
            : base()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        public InternalClientProxy(String endpointConfigurationName)
            : base(endpointConfigurationName)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="remoteAddress"></param>
        public InternalClientProxy(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        { }
    }
}
