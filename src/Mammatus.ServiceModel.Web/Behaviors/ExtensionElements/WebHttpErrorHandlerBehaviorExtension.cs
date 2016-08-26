using System;
using System.Configuration;
using System.ServiceModel.Configuration;
using Mammatus.ServiceModel.Web.Dispatchers;

namespace Mammatus.ServiceModel.Web.Behaviors.ExtensionElements
{
    public class WebHttpErrorHandlerBehaviorExtension : BehaviorExtensionElement
    {
        [ConfigurationProperty("useJsonFault", DefaultValue = false, IsRequired = true)]
        public bool UseJsonFault
        {
            get { return (bool)this["useJsonFault"]; }
            set { this["useJsonFault"] = value; }
        }

        private ConfigurationPropertyCollection _propertyCollection = null;

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                if (_propertyCollection == null)
                {
                    _propertyCollection = new ConfigurationPropertyCollection
                    {
                        new ConfigurationProperty("useJsonFault", typeof(bool), false, ConfigurationPropertyOptions.IsRequired)
                    };
                }
                return this._propertyCollection;
            }
        }

        public override Type BehaviorType
        {
            get { return typeof(WebHttpErrorHandlerDispatcher); }
        }

        protected override object CreateBehavior()
        {
            return new WebHttpErrorHandlerDispatcher(UseJsonFault);
        }
    }
}