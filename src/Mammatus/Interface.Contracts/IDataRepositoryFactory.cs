using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Interface.Contracts
{
    public interface IDataRepositoryFactory
    {
        T GetDataRepository<T>() where T : IDataRepository;
    }
}
