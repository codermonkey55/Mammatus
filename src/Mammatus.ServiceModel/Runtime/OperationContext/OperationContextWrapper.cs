using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using Mammatus.Core.Application;
using Mammatus.ServiceModel.Runtime.InstanceContext;
using Mammatus.ServiceModel.Runtime.ServiceHost;

namespace Mammatus.ServiceModel.Runtime.OperationContext
{
    public class OperationContextWrapper : ObjectBase<OperationContextWrapper>, IOperationContextWrapper
    {
        readonly System.ServiceModel.OperationContext _context;

        readonly IServiceHostWrapper _serviceHostWrapper;

        readonly IInstanceContextWrapper _instanceContext;

        public OperationContextWrapper(System.ServiceModel.OperationContext context)
        {
            _context = context;

            _instanceContext = InstanceContextWrapper.Create(context.InstanceContext);
        }

        public OperationContextWrapper(System.ServiceModel.OperationContext context, IServiceHostWrapper serviceHostWrapper)
        {
            _context = context;

            _serviceHostWrapper = serviceHostWrapper;
        }

        public OperationContextWrapper(System.ServiceModel.OperationContext context, IInstanceContextWrapper instanceContext)
        {
            _context = context;

            _instanceContext = instanceContext;
        }

        public OperationContextWrapper(System.ServiceModel.OperationContext context, IInstanceContextWrapper instanceContext, IServiceHostWrapper serviceHostWrapper)
        {
            _context = context;

            _serviceHostWrapper = serviceHostWrapper;

            _instanceContext = InstanceContextWrapper.Create(context.InstanceContext);
        }

        public IContextChannel Channel
        {
            get { return _context.Channel; }
        }

        public EndpointDispatcher EndpointDispatcher
        {
            get { return _context.EndpointDispatcher; }
        }

        public IExtensionCollection<System.ServiceModel.OperationContext> Extensions
        {
            get { return _context.Extensions; }
        }

        public bool HasSupportingTokens
        {
            get { return _context.HasSupportingTokens; }
        }

        public IServiceHostWrapper Host
        {
            get { return ServiceHostWrapper.Create(_context.Host); }
        }

        public MessageHeaders IncomingMessageHeaders
        {
            get { return _context.IncomingMessageHeaders; }
        }

        public MessageProperties IncomingMessageProperties
        {
            get { return _context.IncomingMessageProperties; }
        }

        public MessageVersion IncomingMessageVersion
        {
            get { return _context.IncomingMessageVersion; }
        }

        public IInstanceContextWrapper InstanceContext
        {
            get { return _instanceContext; }
        }

        public MessageHeaders OutgoingMessageHeaders
        {
            get { return _context.OutgoingMessageHeaders; }
        }

        public MessageProperties OutgoingMessageProperties
        {
            get { return _context.OutgoingMessageProperties; }
        }

        public RequestContext RequestContext
        {
            get { return _context.RequestContext; }
        }

        public string SessionId
        {
            get { return _context.SessionId; }
        }

        public ICollection<SupportingTokenSpecification> SupportingTokens
        {
            get { return _context.SupportingTokens; }
        }

        public T GetCallbackChannel<T>()
        {
            return _context.GetCallbackChannel<T>();
        }

        public void SetTransactionComplete()
        {
            _context.SetTransactionComplete();
        }
    }
}