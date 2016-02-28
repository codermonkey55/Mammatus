using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Data.Entity
{
    public class TreeEntity<TKey> : EntityBase<TKey>, ITreeEntity<TKey>
         where TKey : struct
    {
        public TKey? ParentId { get; set; }
    }
}
