using Mammatus.Data.NHibernate.Entities;
using NHibernate.Event;
using NHibernate.Event.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Data.NHibernate.Listeners
{
    public class AuditFieldsDirtyCheckingEventListener : DefaultFlushEntityEventListener
    {
        protected override void DirtyCheck(FlushEntityEvent e)
        {
            base.DirtyCheck(e);
            if (e.DirtyProperties != null &&
                e.DirtyProperties.Any() &&
                //IAuditableEntity is my inteface for audited entities
                e.Entity is IAuditableEntity)
                e.DirtyProperties = e.DirtyProperties
                 .Concat(GetAdditionalDirtyProperties(e)).ToArray();
        }

        protected virtual IEnumerable<int> GetAdditionalDirtyProperties(FlushEntityEvent @event)
        {
            yield return Array.IndexOf(@event.EntityEntry.Persister.PropertyNames, "CreateDate");

            yield return Array.IndexOf(@event.EntityEntry.Persister.PropertyNames, "EditDate");
        }
    }
}
