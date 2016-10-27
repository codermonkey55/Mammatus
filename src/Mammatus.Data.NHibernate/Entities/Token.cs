using System;
using System.Collections.Generic;

namespace Mammatus.Data.NHibernate.Entities
{
    public class Token : Base.Entity<Int32>
    {
        public Token()
        {
            Users = new HashSet<User>();
        }

        public virtual Guid AuthKey { get; set; }

        public virtual User User { get; set; }

        public virtual ISet<User> Users { get; protected set; }
    }
}
