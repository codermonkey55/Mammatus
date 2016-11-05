using Mammatus.ServiceModel.IntegrationTest.ServiceHostFactories;
using Mammatus.ServiceModel.IntegrationTest.Services;
using System;
using System.ServiceModel;

namespace Mammatus.ServiceModel.IntegrationTest.Setups
{
    class TestServiceHostSetup
    {
        internal static ServiceHost InitServiceHost(string baseAddressString)
        {
            var baseAddressUri = new Uri(baseAddressString);

            var serviceHostFactory = new TestDependencyInjectionServiceHostFactory();

            var serviceHost = serviceHostFactory.GetServiceHost(typeof(TestService), new Uri[] { });

            serviceHost.Open();

            return serviceHost;
        }

        internal static void TerminateServiceHost(ServiceHost host)
        {
            try
            {
                if (host.State == CommunicationState.Opened)
                    host.Close();
            }
            catch (Exception)
            {

                host.Abort();
            }
        }
    }
}
