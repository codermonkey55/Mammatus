namespace Mammatus.ServiceModel.IntegrationTest.ServiceContracts
{
    internal interface ITestService
    {
        [OperationContract]
        void ThrowUnhandledException();

        [OperationContract]
        void ExecuteLongRunningOperation();

        [OperationContract]
        [WebInvoke(UriTemplate = "/UnhandledErrorWithDetails",
                   Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        void ThrowUnhandledExceptionWithInnerException();
    }
}
