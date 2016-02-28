using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammatus.Core.Application
{
    public abstract class ObjectBase
    {
        protected virtual TConcrete Create<TConcrete>()
            where TConcrete : class, new()
        {
            throw new InvalidOperationException("Override and implement in derived type.");
        }
    }
}
