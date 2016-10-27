namespace Mammatus.Data.NHibernate.Entities.Base
{
    public abstract class Entity<TKey> : AuditableEntity where TKey : struct
    {
        public virtual TKey Id { get; protected set; }
    }
}
