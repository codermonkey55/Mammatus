using System.ServiceModel.Configuration;

namespace Mammatus.ServiceModel.Extensibility.Behaviors.ExtensionElements
{
    public abstract class BehaviorExtensionElementBase<TServiceBehavior> : BehaviorExtensionElement
        where TServiceBehavior : class, new()
    {
        public override System.Type BehaviorType
        {
            get { return typeof(TServiceBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new TServiceBehavior();
        }
    }
}
