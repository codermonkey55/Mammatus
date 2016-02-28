using System;
using System.Linq;

namespace Mammatus.Domain.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ISubscriptionService _subscriptionService;

        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public void Publish<T>(T eventMessage)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<T>();

            subscriptions.ToList().ForEach(x => PublishToConsumer(x, eventMessage));
        }

        private void PublishToConsumer<T>(IEventConsumer<T> x, T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception)
            {
                //inlcuded in nested try-catch to prevent possible cyclic (if some error occurs)
                try
                {
                    // TODO: Log Error.
                }
                catch
                {

                }
            }
            finally
            {

            }
        }
    }
}
