using System;
using Mammatus.ServiceModel.Extensibility.Behaviors.Service;
using Mammatus.ServiceModel.Extensibility.DependencyInjection.Resolvers;

namespace Mammatus.ServiceModel.Extensibility.Hosts
{
    public sealed class IoCServiceHost : ServiceHostBase
    {
        private readonly IServiceResolver _serviceResolver;

        public IServiceResolver DependencyResolver { get { return _serviceResolver; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyInjectionServiceHost"/> class.
        /// </summary>
        /// <param name="serviceResolver"></param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public IoCServiceHost(IServiceResolver serviceResolver, System.Type serviceType, Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            //Register the service as a type so it can be found from the instance provider
            _serviceResolver.Register(serviceType);

            _serviceResolver = serviceResolver;
        }

        /// <summary>
        /// Opens the channel dispatchers.
        /// </summary>
        /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the on-open operation has to complete before timing out.</param>
        protected override void OnOpen(TimeSpan timeout)
        {
            Description.Behaviors.Add(new IoCServiceBehavior());

            base.OnOpen(timeout);
        }
    }
}
