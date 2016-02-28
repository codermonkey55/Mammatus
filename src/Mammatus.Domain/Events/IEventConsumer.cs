
namespace Mammatus.Domain.Events
{
    public interface IEventConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}
