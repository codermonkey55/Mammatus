
namespace Mammatus.Domain.Events
{
    public class EntityDeleted<T>
    {
        public EntityDeleted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
