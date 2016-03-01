using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Interface.Contracts
{
    public interface IServiceClientFactory
    {
        T CreateClient<T>() where T : IServiceContract;
    }
}