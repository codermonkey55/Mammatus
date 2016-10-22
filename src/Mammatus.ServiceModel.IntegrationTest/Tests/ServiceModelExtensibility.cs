using Mammatus.ServiceModel.IntegrationTest.ServiceContracts;
using Mammatus.ServiceModel.IntegrationTest.Setups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Mammatus.ServiceModel.IntegrationTest
{
    [TestClass]
    public class ServiceModelExtensibilityTests : TestClassSetup
    {
        [ClassInitialize]
        public static void TestClassInitialize(TestContext testContext)
        {
            SetupTestClass(testContext);
        }

        [ClassCleanup]
        public static void TestClassCleanUp()
        {
            TestServiceHostSetup.TerminateServiceHost(Host);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void ErrorHandldingBehavior_ReturnFaultException_VerifyChannelNotFaulted()
        {
            var client = GetClient<ITestService>(endPointAddress: "/soap");

            try
            {
                client.ThrowUnhandledException();
            }
            catch (System.ServiceModel.FaultException e)
            {
                Assert.IsNotNull(e);
                Assert.IsTrue(((ICommunicationObject)client).State == CommunicationState.Opened);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void OperationMetricsBehvavior_LogOperationExecutionTime_VerifyLogCreated()
        {
            var appLocalPath = AppDomain.CurrentDomain.BaseDirectory;

            var serviceMetricsLogFilePath = Path.Combine(appLocalPath, "Logs/Service");

            if (Directory.Exists(serviceMetricsLogFilePath))
                Directory.Delete(serviceMetricsLogFilePath, true);

            var client = GetClient<ITestService>(endPointAddress: "/soap");

            client.ExecuteLongRunningOperation();

            Assert.IsTrue(Directory.Exists(serviceMetricsLogFilePath));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void WebHttpErrorHandling_ReturnJsonErrorDetails_VerifyJsonResposne()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(BaseAddress);

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var requestUri = BaseAddress + "/UnhandledErrorWithDetails";

                var jsonResponse = client.GetAsync(requestUri).Result;

                var hasContentTypeHeader = jsonResponse.Content.Headers.Contains("Content-Type");

                var isJsonResposne = false;
                if (hasContentTypeHeader)
                    isJsonResposne = jsonResponse.Content.Headers.GetValues("Content-Type").Contains("application/json");

                var replyMessage = jsonResponse.Content.ReadAsStringAsync().Result;

                Assert.IsTrue(hasContentTypeHeader);
                Assert.IsTrue(isJsonResposne);
                Assert.IsTrue(!string.IsNullOrEmpty(replyMessage));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

        }

        internal TChannel GetClient<TChannel>(string endPointAddress = "")
        {
            var basicHttpBinding = new BasicHttpBinding();

            var endpointAddress = new EndpointAddress(BaseAddress + endPointAddress);

            var channelFactory = new ChannelFactory<TChannel>(basicHttpBinding, endpointAddress);

            return channelFactory.CreateChannel();
        }
    }
}
