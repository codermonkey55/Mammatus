using System;
using System.Linq;

namespace Mammatus.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void RollbackChanges();
    }
}