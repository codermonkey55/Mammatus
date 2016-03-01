using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Interface.Contracts
{
    public interface IIdentifiableEntity
    {
        int EntityId { get; set; }
    }
}
