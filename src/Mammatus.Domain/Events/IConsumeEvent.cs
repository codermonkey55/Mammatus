namespace Mammatus.Domain.Events
{
    /// <summary>
    /// Event consumer contract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConsumeEvent<T>
        where T : class
    {
        /// <summary>
        /// Consumes the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        void Consume(T @event);
    }
}