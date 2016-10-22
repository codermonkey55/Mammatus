using System;

namespace Mammatus.ServiceModel.IntegrationTest.ServiceHostFactories
{
    internal class TestDependencyInjectionServiceHostFactory : IoCServiceHostFactory
    {
        public TestDependencyInjectionServiceHostFactory()
        {

        }


        /// <summary>
        /// Re-evaluate whether IoCServiceHostFactory should be sealed or open...
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="baseAddresses"></param>
        /// <returns></returns>
        public ServiceHost GetServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return base.CreateServiceHost(serviceType, baseAddresses);
        }
    }
}
