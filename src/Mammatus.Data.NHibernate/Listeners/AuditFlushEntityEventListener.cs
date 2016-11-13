using Mammatus.Domain.Core;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Event.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Data.NHibernate.Listeners
{
    /// <summary>
    ///
    /// </summary>
    /// http://stackoverflow.com/questions/5087888/ipreupdateeventlistener-and-dynamic-update-true
    public class AuditFlushEntityEventListener : DefaultFlushEntityEventListener
    {
        protected override void DirtyCheck(FlushEntityEvent @event)
        {
            base.DirtyCheck(@event);
            if (@event.DirtyProperties != null &&
                @event.DirtyProperties.Any() &&
                @event.Entity is IAuditableEntity)
            {
                @event.DirtyProperties = @event.DirtyProperties.Concat(GetAdditionalDirtyProperties(@event)).ToArray();
            }
        }

        private static IEnumerable<int> GetAdditionalDirtyProperties(FlushEntityEvent @event)
        {
            yield return Array.IndexOf(
                @event.EntityEntry.Persister.PropertyNames,
                "CreatedBy");
            yield return Array.IndexOf(
                @event.EntityEntry.Persister.PropertyNames,
                "CreatedAt");
            yield return Array.IndexOf(
                @event.EntityEntry.Persister.PropertyNames,
                "UpdatedAt");
            yield return Array.IndexOf(
                @event.EntityEntry.Persister.PropertyNames,
                "UpdatedBy");
        }

        public static void OverrideIn(Configuration configuration)
        {
            configuration.SetListeners(
                ListenerType.FlushEntity,
                new IFlushEntityEventListener[] { new AuditFlushEntityEventListener() });
        }
    }
}