
namespace Mammatus.Domain.Events
{
    public class EntityInserted<T>
    {
        public EntityInserted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
