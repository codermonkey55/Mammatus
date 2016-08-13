using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Mammatus.ServiceModel.Extensibility.Dispatchers
{
    public sealed class SoapErrorHandlerDispatcher : ErrorHandlerDispatcherBase
    {
        public override bool HandleError(Exception error)
        {
            return true;
        }

        public override void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var operationAction = OperationContext.Current.IncomingMessageHeaders.Action;
            var operationName = OperationContext.Current.IncomingMessageHeaders.Action.Split('/').ToList().Last();

            DispatchOperation operation = OperationContext.Current
                                                          .EndpointDispatcher
                                                          .DispatchRuntime
                                                          .Operations
                                                          .FirstOrDefault(o => o.Action == operationAction);

            MethodInfo method;

            if (operation != null)
                method = GetActionMethodInfo(OperationContext.Current);

            if (error is ArgumentException)
            {
                FaultException<ArgumentException> faultException = new FaultException<ArgumentException>(new ArgumentException(error.Message));

                fault = Message.CreateMessage(version, faultException.CreateMessageFault(), operationName);
            }
        }

        ///<summary>Returns the Method info for the method (OperationContract) that is called in this WCF request.</summary>
        System.Reflection.MethodInfo GetActionMethodInfo(OperationContext operationContext)
        {
            string bindingName = operationContext.EndpointDispatcher.ChannelDispatcher.BindingName;

            string methodName = string.Empty;

            if (bindingName.Contains("WebHttpBinding"))
            {
                //REST request
                methodName = (string)operationContext.IncomingMessageProperties["HttpOperationName"];
            }
            else
            {
                //SOAP request
                string action = operationContext.IncomingMessageHeaders.Action;

                var operationDescription = operationContext.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o => o.Action == action);

                if (operationDescription != null)
                    methodName = operationDescription.Name;
            }

            System.Type hostType = operationContext.Host.Description.ServiceType;
            return hostType.GetMethod(methodName);
        }
    }
}
