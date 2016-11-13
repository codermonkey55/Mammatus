using Mammatus.Core.Object;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;

namespace Mammatus.Data.NHibernate.Listeners
{
    public class AuditingEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {

        public bool OnPreInsert(PreInsertEvent @event)
        {
            SetAuditFields(@event.Entity as IAuditableEntity, "Insert");
            CommitAuditInfo(@event.Persister, @event.State, "CreateDate");

            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            SetAuditFields(@event.Entity as IAuditableEntity, "Update");
            CommitAuditInfo(@event.Persister, @event.State, "EditDate");

            return false;
        }

        private void SetAuditFields(IAuditableEntity auditableEntity, string writeEvent)
        {
            if (auditableEntity == null) return;

            switch (writeEvent)
            {
                case "Insert":
                    auditableEntity.CreateDate = DateTime.UtcNow;
                    break;

                case "Update":
                    auditableEntity.EditDate = DateTime.UtcNow;
                    break;
            }
        }

        private void CommitAuditInfo(IEntityPersister persister, object[] state, string propertyName)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);

            if (index == -1) return;

            state[index] = DateTime.UtcNow;
        }
    }
}
