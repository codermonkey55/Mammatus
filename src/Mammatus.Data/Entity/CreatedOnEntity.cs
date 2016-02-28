using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Data.Entity
{
    public abstract partial class CreatedOnEntity<TKey> : EntityBase<TKey>
        where TKey : struct
    {
        protected CreatedOnEntity()
        {
            this.CreatedOn = DateTime.Now;
        }

        public DateTime CreatedOn { get; set; }
    }

}
