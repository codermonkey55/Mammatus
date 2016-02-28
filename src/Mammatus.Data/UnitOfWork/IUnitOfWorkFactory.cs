using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Mammatus.Data.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
