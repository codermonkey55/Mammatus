using Mammatus.Data.Contracts;
using System.Data.Entity;

namespace Mammatus.Data.Entity
{
    public abstract class EFDomainRepository : IPersistableRepository
    {
        protected readonly DbContext _dbContext;

        public EFDomainRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Modify(object entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public int Persist()
        {
            return _dbContext.SaveChanges();
        }
    }
}
