using Mammatus.ServiceModel.IntegrationTest.ServiceContracts;
using System;
using System.Threading;

namespace Mammatus.ServiceModel.IntegrationTest.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, AddressFilterMode = AddressFilterMode.Any, IncludeExceptionDetailInFaults = true)]
    internal sealed class TestService : ITestService
    {
        [MetricsOperationBahavior]
        public void ExecuteLongRunningOperation()
        {
            Thread.Sleep(6000); //-> Sleep for 6 seconds.
        }

        public void ThrowUnhandledException()
        {
            throw new NotImplementedException();
        }

        public void ThrowUnhandledExceptionWithInnerException()
        {
            throw new InvalidOperationException("Invalid Operaiton", new Exception("Inner Exception"));
        }
    }
}
