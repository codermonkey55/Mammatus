using System.ServiceModel;
using System.ServiceModel.Channels;
using Mammatus.ServiceModel.Extensibility.Hosts;

namespace Mammatus.ServiceModel.Extensibility.Providers
{
    /// <summary>
    /// A custom instance provider that uses the ServiceLocator from MS Pattern and Practices to resolve service types.
    /// </summary>
    /// <remarks>
    public class IoCInstanceProvider : InstanceProviderBase
    {
        private readonly System.Type _serviceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionInstanceProvider"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public IoCInstanceProvider(System.Type serviceType)
        {
            _serviceType = serviceType;
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>A user-defined service object.</returns>
        public override object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        public override object GetInstance(InstanceContext instanceContext, Message message)
        {
            var ioCServiceHost = instanceContext.Host as IoCServiceHost;

            if (ioCServiceHost != null)
            {
                var dependencyResolver = ioCServiceHost.DependencyResolver;

                return dependencyResolver.Resolve(_serviceType);
            }

            return base.GetInstance(instanceContext, message);
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public override void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}
