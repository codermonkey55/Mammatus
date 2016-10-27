using System;

namespace Mammatus.Data.NHibernate.Entities
{
    public interface IAuditableEntity
    {
        DateTime CreateDate { get; set; }

        DateTime? EditDate { get; set; }
    }
}
