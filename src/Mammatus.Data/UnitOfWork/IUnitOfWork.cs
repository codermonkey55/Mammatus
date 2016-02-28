﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Mammatus.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Flush();
    }
}
