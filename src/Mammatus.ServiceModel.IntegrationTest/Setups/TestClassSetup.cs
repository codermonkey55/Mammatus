using Autofac;
using Mammatus.ServiceModel.IntegrationTest.Bootstrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;

namespace Mammatus.ServiceModel.IntegrationTest.Setups
{
    public class TestClassSetup
    {
        protected static string BaseAddress = "http://localhost:8733/Design_Time_Addresses/PocoLib.ServiceModel.InterationTest/TestService";

        protected static ServiceHost Host { get; set; }

        protected static IContainer Container { get; set; }

        protected static TestContext TestContext { get; set; }

        static TestClassSetup()
        {

        }

        internal TestClassSetup()
        {

        }

        internal static void SetupTestClass(TestContext testContext)
        {
            TestContext = testContext;

            TestBootstrapper.Configure();

            try
            {
                Host = TestServiceHostSetup.InitServiceHost(BaseAddress);
            }
            catch (System.ServiceModel.AddressAccessDeniedException)
            {
                Assert.Inconclusive("Test service endpoint adddress not registered.");
            }
        }
    }
}
