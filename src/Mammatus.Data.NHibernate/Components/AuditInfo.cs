using System;

namespace Mammatus.Data.NHibernate.Entities.Components
{
    public class AuditInfo
    {
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? EditDate { get; set; }
    }
}
