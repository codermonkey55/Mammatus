using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace Mammatus.ServiceModel.Core
{
    public abstract class ServiceClientBase<T> : ClientBase<T> where T : class
    {
        public ServiceClientBase()
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;
            MessageHeader<string> header = new MessageHeader<string>(userName);
            OperationContextScope contextScope = new OperationContextScope(InnerChannel);
            OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));
        }
    }
}
