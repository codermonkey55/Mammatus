using Mammatus.Domain.UnitOfWork;
using NHibernate;

namespace Mammatus.Data.NHibernate.DbOperations
{
    public interface INHUnitOfWork : INestableUnitOfWork
    {
        ISession Session { get; }
    }
}