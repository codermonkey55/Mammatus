using System;
using System.Linq;
using System.Collections.Generic;

namespace Mammatus.Data.Entity
{
    public interface IEntity<TKey> : IEntity
    {
        new TKey Id { get; set; }
    }
}