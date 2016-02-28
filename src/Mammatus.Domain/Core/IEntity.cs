using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Domain.Core
{
    public interface IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        TKey Id
        {
            get;
        }
    }
}
