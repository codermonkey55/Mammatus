
namespace Mammatus.Domain.Events
{
    public class EntityUpdated<T>
    {
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
