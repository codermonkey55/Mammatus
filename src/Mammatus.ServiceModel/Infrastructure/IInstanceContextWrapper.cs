using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace Mammatus.ServiceModel.State
{
    public interface IInstanceContextWrapper
    {
        IExtensionCollection<InstanceContext> Extensions { get; }

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