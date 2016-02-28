
namespace Mammatus.Domain.Events
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}
