using Mammatus.Data.Contracts;
using Mammatus.Data.Entity.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Mammatus.Data.Entity.Core
{
    public interface IEFEntityCollection : IEntityCollection
    {
        DbSet<TEntity> Collection<TEntity>() where TEntity : class;
    }

    public interface IEFEntityCollection<T> : IEFEntityCollection where T : class
    {
        DbSet<T> Entities { get; }

        void Attach(T entity, EntityState state);

        T Find(object id);
    }

    public interface IEFCollectionPersister : ICollectionPersister
    {
        void Attach(object entity, EntityState modified);
    }

    public class EFEntityCollection : IEFEntityCollection
    {
        protected readonly DbContext _dbContext;

        public EFEntityCollection(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> Collection<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }
    }

    public class EFEntityCollection<T> : EFEntityCollection, IEFEntityCollection<T>, IEFCollectionPersister where T : class
    {
        public EFEntityCollection(DbContext dbContext) : base(dbContext)
        {

        }

        public DbSet<T> Entities
        {
            get
            {
                return _dbContext.Set<T>();
            }
        }

        public void Attach(object entity, EntityState state)
        {
            _dbContext.Entry(entity).State = state;
        }

        public T Find(object id)
        {
            return _dbContext.Find<T>(id);
        }

        public void Attach(T entity, EntityState state)
        {
            Entities.Attach(entity).State = state;
        }

        public int Persist()
        {
            return _dbContext.SaveChanges();
        }
    }
}