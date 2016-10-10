using System.ServiceModel;
using System.Threading;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    public abstract class UserClientBase<T> : ClientBase<T> where T : class
    {
        public UserClientBase()
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;

            MessageHeader<string> header = new MessageHeader<string>(userName);

            OperationContextScope contextScope = new OperationContextScope(InnerChannel);

            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));
        }
    }
}
