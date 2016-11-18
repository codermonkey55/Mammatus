using Mammatus.Domain.UnitOfWork;
using NHibernate;

namespace Mammatus.Data.NHibernate.DbOperations
{
    public interface INHSessionUnitOfWork : INestableUnitOfWork
    {
        ISession Session { get; }
    }
}