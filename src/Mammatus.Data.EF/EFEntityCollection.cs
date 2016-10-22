using Mammatus.Data.Contracts;
using System.Data.Entity;

namespace Mammatus.Data.EF
{
    public interface IEFEntityCollection : IEntityCollection
    {
        IDbSet<TEntity> Collection<TEntity>() where TEntity : class;
    }

    public interface IEFEntityCollection<T> : IEFEntityCollection where T : class
    {
        IDbSet<T> Entities { get; }
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

        public IDbSet<TEntity> Collection<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }
    }

    public class EFEntityCollection<T> : EFEntityCollection, IEFEntityCollection<T>, IEFCollectionPersister where T : class
    {
        public EFEntityCollection(DbContext dbContext) : base(dbContext)
        {

        }

        public IDbSet<T> Entities
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



        public int Persist()
        {
            return _dbContext.SaveChanges();
        }
    }
}