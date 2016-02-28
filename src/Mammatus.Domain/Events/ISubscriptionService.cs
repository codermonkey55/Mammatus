using System.Collections.Generic;

namespace Mammatus.Domain.Events
{
    public interface ISubscriptionService
    {
        IList<IEventConsumer<T>> GetSubscriptions<T>();
    }
}
