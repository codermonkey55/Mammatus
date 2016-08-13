using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using Mammatus.ServiceModel.Web.Faults.Json;

namespace Mammatus.ServiceModel.Web.Dispatchers
{
    public class WebHttpErrorHandlerDispatcher : IEndpointBehavior, IErrorHandler
    {
        private readonly bool _useJsonFault;

        public WebHttpErrorHandlerDispatcher(bool useJsonFault)
        {
            _useJsonFault = useJsonFault;
        }

        #region IEndpoint Support

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();

            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(this);
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        #endregion

        #region IErrorHandler Support

        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var operationAction = OperationContext.Current.IncomingMessageHeaders.Action;

            if (operationAction == null)
            {
                var requestBinding = OperationContext.Current.EndpointDispatcher.ChannelDispatcher.BindingName;
                if (requestBinding.Contains("WebHttpBinding"))
                    operationAction = OperationContext.Current.IncomingMessageProperties["HttpOperationName"] as string;
            }

            DispatchOperation operation = OperationContext.Current
                                               .EndpointDispatcher
                                               .DispatchRuntime
                                               .Operations
                                               .FirstOrDefault(o => o.Action == operationAction);

            if (_useJsonFault)
            {
                var details = error.InnerException != null
                    ? "InnerException available, see server logs for further details"
                    : string.Empty;

                var webHttpError = new WebHttpError { Error = "Unknown Exception", Detail = details };

                var webFaultException = new WebFaultException<WebHttpError>(webHttpError,
                    HttpStatusCode.InternalServerError);

                fault = Message.CreateMessage(version, operationAction, webFaultException);
            }
            else
            {
                var detail = error as FaultException;

                var faultDetail = detail ?? new FaultException(new FaultReason("Unknown Exception"), FaultCode.CreateReceiverFaultCode("Unhandled", ""));

                fault = Message.CreateMessage(version, operationAction, faultDetail, new DataContractJsonSerializer(faultDetail.GetType()));
            }

            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);

            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

            var rmp = new HttpResponseMessageProperty
            {
                StatusCode = HttpStatusCode.InternalServerError,

                StatusDescription = "See server logs for further details"
            };

            rmp.Headers[HttpResponseHeader.ContentType] = "application/json";

            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);

            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
        }

        #endregion
    }
}