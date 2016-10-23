using Mammatus.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Mammatus.Data.Entity.Core
{
    public class EFUnitOfWork : IUnitOfWork<IEFRepository>
    {
        private DbContext _dbContext;

        private readonly IDictionary<Type, IEFRepository> _repositories;

        public EFUnitOfWork(DbContext context)
        {
            _dbContext = context;

            _repositories = new Dictionary<Type, IEFRepository>();
        }

        public IEFRepository this[Type repoType]
        {
            get
            {
                IEFRepository repo = EFRepository.Empty;

                if (_repositories.ContainsKey(repoType))
                    repo = _repositories[repoType];

                return repo;
            }
        }

        public void Enroll<TDerivedRepo>(TDerivedRepo repository)
            where TDerivedRepo : IEFRepository
        {
            if (repository == null)
                throw new ArgumentNullException(typeof(TDerivedRepo).Name);

            var entityCollection = new EFEntityCollection(_dbContext);

            ((ISessionRepository)repository).SetCollection(entityCollection);

            _repositories[typeof(TDerivedRepo)] = repository;
        }


        public int Commit()
        {
            //-> Save changes with the default options.
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();

                    _dbContext = null;
                }

                if (_repositories != null)
                {
                    _repositories.Clear();
                }
            }
        }
    }
}
