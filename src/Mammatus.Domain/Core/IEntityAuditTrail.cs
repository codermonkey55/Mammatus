
namespace Mammatus.Domain.Core
{
    using System;

    public interface IEntityAuditTrail
    {
        Guid ChangeSetUniqueId
        {
            get;
            set;
        }

        string EntityUniqueId
        {
            get;
            set;
        }

        object NewValue
        {
            get;
            set;
        }

        object OldValue
        {
            get;
            set;
        }

        string PropertyName
        {
            get;
            set;
        }

        string UpdateBy
        {
            get;
            set;
        }

        DateTime UpdatedAt
        {
            get;
            set;
        }
    }
}