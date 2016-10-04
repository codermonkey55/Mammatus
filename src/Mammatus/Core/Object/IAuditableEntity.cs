using System;

namespace Mammatus.Core.Object
{
    public interface IAuditableEntity
    {
        DateTime CreateDate { get; set; }

        DateTime? EditDate { get; set; }

    }

    public interface IAuditableEntity<TUser>
    {
        DateTime CreateDate { get; set; }

        TUser CreatedUser { get; set; }

        DateTime? EditDate { get; set; }

        TUser EditUser { get; set; }
    }
}
