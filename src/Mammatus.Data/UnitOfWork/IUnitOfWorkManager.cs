using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Data.UnitOfWork
{
    public interface IUnitOfWorkManager : IDisposable
    {
        IUnitOfWork CurrentUnitOfWork { get; }

        IUnitOfWorkScope CurrentScope { get; }

        IUnitOfWorkScope BeginScope();

        void EndScope(IUnitOfWorkScope scope);
    }
}
