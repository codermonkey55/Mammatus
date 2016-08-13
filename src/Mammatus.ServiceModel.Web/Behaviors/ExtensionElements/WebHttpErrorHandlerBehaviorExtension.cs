using System;
using System.ServiceModel.Configuration;
using Mammatus.ServiceModel.Web.Dispatchers;

namespace Mammatus.ServiceModel.Web.Behaviors.ExtensionElements
{
    public class WebHttpErrorHandlerBehaviorExtension : BehaviorExtensionElement
    {
        private readonly bool _useJsonFault;

        public WebHttpErrorHandlerBehaviorExtension() { }

        public WebHttpErrorHandlerBehaviorExtension(bool useJsonFault)
        {
            _useJsonFault = useJsonFault;
        }

        public override Type BehaviorType
        {
            get { return typeof(WebHttpErrorHandlerDispatcher); }
        }

        protected override object CreateBehavior()
        {
            return new WebHttpErrorHandlerDispatcher(_useJsonFault);
        }
    }
}