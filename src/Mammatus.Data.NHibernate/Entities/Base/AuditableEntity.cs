using System;

namespace Mammatus.Data.NHibernate.Entities.Base
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? EditDate { get; set; }

        public virtual Components.AuditInfo AuditInfo { get; set; }
    }
}
