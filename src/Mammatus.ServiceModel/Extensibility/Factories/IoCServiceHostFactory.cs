using System;
using System.ServiceModel;
using Mammatus.ServiceModel.Extensibility.DependencyInjection.Providers;
using Mammatus.ServiceModel.Extensibility.Hosts;

namespace Mammatus.ServiceModel.Extensibility.Factories
{
    public sealed class IoCServiceHostFactory : ServiceHostFactoryBase
    {
        public IoCServiceHostFactory() { }

        /// <summary>
        /// Creates a <see cref="IoCServiceHost"/> for a specified type of service with a specific base address. 
        /// </summary>
        /// <returns>
        /// A <see cref="IoCServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        /// <param name="serviceType">
        /// Specifies the type of service to host. 
        /// </param>
        /// <param name="baseAddresses">
        /// The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.
        /// </param>
        protected override ServiceHost CreateServiceHost(System.Type serviceType, Uri[] baseAddresses)
        {

            var serviceResolver = ServiceResolverProvider.Current.GetResolver();

            var host = serviceResolver == null ? base.CreateServiceHost(serviceType, baseAddresses)
                                               : new IoCServiceHost(serviceResolver, serviceType, baseAddresses);

            return host;
        }
    }
}
