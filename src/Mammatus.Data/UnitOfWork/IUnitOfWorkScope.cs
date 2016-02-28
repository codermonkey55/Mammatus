
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Data.UnitOfWork
{
    using System;

    public interface IUnitOfWorkScope : IDisposable
    {
        Guid ScopeId { get; }

        void Commit();

        IUnitOfWork UnitOfWork { get; }
    }
}
