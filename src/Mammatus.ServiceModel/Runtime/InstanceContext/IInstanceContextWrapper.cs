using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using Mammatus.ServiceModel.Runtime.ServiceHost;

namespace Mammatus.ServiceModel.Runtime.InstanceContext
{
    public interface IInstanceContextWrapper
    {
        IExtensionCollection<System.ServiceModel.InstanceContext> Extensions { get; }

        IServiceHostWrapper Host { get; }

        ICollection<IChannel> IncomingChannels { get; }

        int ManualFlowControlLimit { get; set; }

        ICollection<IChannel> OutgoinChannels { get; }

        SynchronizationContext SynchronizationContext { get; set; }

        void IncrementManualFlowControlLimit(int limit);

        object GetServiceInstance();

        object GetServiceInstance(Message message);
    }
}