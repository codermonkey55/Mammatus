using Mammatus.Domain.UnitOfWork;
using NHibernate;

namespace Mammatus.Data.NHibernate.DbOperations
{
    public interface INHStatelessUnitOfWork : INestableUnitOfWork
    {
        IStatelessSession Session { get; }
    }
}