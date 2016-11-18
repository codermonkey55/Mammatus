using System;

namespace Mammatus.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void EndTransaction();

        void Commit();

        void RollbackChanges();
    }
}