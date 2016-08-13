using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using Mammatus.Core.Application;
using Mammatus.ServiceModel.Runtime.ServiceHost;

namespace Mammatus.ServiceModel.Runtime.InstanceContext
{
    public sealed class InstanceContextWrapper : ObjectBase<InstanceContextWrapper>, IInstanceContextWrapper
    {
        readonly System.ServiceModel.InstanceContext _context;

        readonly IServiceHostWrapper _serviceHostWrapper;

        public InstanceContextWrapper(System.ServiceModel.InstanceContext context)
        {
            _context = context;

            _serviceHostWrapper = ServiceHostWrapper.Create(_context.Host);
        }

        public InstanceContextWrapper(System.ServiceModel.InstanceContext context, IServiceHostWrapper serviceHostWrapper)
        {
            _context = context;

            _serviceHostWrapper = serviceHostWrapper;
        }


        public IExtensionCollection<System.ServiceModel.InstanceContext> Extensions
        {
            get { return _context.Extensions; }
        }

        public IServiceHostWrapper Host
        {
            get { return _serviceHostWrapper; }
        }

        public ICollection<IChannel> IncomingChannels
        {
            get { return _context.IncomingChannels; }
        }

        public int ManualFlowControlLimit
        {
            get { return _context.ManualFlowControlLimit; }
            set { _context.ManualFlowControlLimit = value; }
        }

        public ICollection<IChannel> OutgoinChannels
        {
            get { return _context.OutgoingChannels; }
        }

        public SynchronizationContext SynchronizationContext
        {
            get { return _context.SynchronizationContext; }
            set { _context.SynchronizationContext = value; }
        }

        public void IncrementManualFlowControlLimit(int limit)
        {
            _context.IncrementManualFlowControlLimit(limit);
        }

        public object GetServiceInstance()
        {
            return _context.GetServiceInstance();
        }

        public object GetServiceInstance(Message message)
        {
            return _context.GetServiceInstance(message);
        }
    }
}