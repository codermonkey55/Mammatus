using System.ServiceModel;
using System.Threading;

namespace Mammatus.ServiceModel.Core
{
    public abstract class ServiceClientBase<T> : ClientBase<T> where T : class
    {
        protected ServiceClientBase()
        {
            InitIdentityHeader();
        }

        protected ServiceClientBase(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            InitIdentityHeader();
        }

        protected void InitIdentityHeader()
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;

            MessageHeader<string> header = new MessageHeader<string>(userName);

            OperationContextScope contextScope = new OperationContextScope(InnerChannel);

            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));
        }
    }
}
