using Mammatus.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Mammatus.Data.Entity.Core
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
