using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Domain.Events
{
    public class SubscriptionService : ISubscriptionService
    {
        protected IEventConsumerFactory _eventConsumerFactory;

        public SubscriptionService(IEventConsumerFactory eventConsumerFactory)
        {
            _eventConsumerFactory = eventConsumerFactory;
        }
        public IList<IEventConsumer<T>> GetSubscriptions<T>()
        {
            var consumers = _eventConsumerFactory.GetAllEventConsumers<T>();

            var items = consumers.ToList();

            return items;
        }
    }
}
