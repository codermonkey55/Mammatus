using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Data.Entity
{
    public interface ITreeEntity<TKey> : IEntity<TKey>
        where TKey : struct
    {
        TKey? ParentId { get; set; }
    }
}
