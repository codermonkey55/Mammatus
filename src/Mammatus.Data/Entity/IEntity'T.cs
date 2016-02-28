using System;
using System.Linq;
using System.Collections.Generic;

namespace Mammatus.Data.Entity
{
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}