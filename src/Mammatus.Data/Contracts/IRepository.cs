using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mammatus.Data.Contracts
{
    public interface IRepository
    {

    }

    public interface IRepository<T> : IRepository where T : class
    {
        T Add(T entity);
        T GetById(object id);
        T Delete(T entity);
        void Edit(T entity);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
    }

    public interface IDomainRepository : IRepository
    {

    }
}
