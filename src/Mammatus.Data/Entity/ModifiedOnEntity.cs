using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Data.Entity
{
    public abstract partial class ModifiedOnEntity<TKey> : CreatedOnEntity<TKey>
        where TKey : struct
    {
        protected ModifiedOnEntity()
            : base()
        {
            this.ModifiedOn = this.CreatedOn;
        }

        public DateTime ModifiedOn { get; set; }
    }
}
