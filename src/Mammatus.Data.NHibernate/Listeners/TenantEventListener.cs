﻿using Mammatus.Core.State;
using Mammatus.Domain.Core;
using Mammatus.Exceptions;
using Mammatus.Security;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;
using System.Linq;

namespace Mammatus.Data.NHibernate.Listeners
{
    public class TenantEventListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            var tenantScoped = @event.Entity as ITenantScopedEntity;
            if (tenantScoped == null)
            {
                return false;
            }

            var tenant = ApplicationContext.User as TenantPrincipal;
            if (tenant == null)
            {
                throw new InternalException("No tenant principal in context");
            }

            this.Set(@event.Persister, @event.State, "TenantId", tenant.TenantId);

            tenantScoped.SetTenantId(tenant.TenantId);

            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var tenantScoped = @event.Entity as ITenantScopedEntity;
            if (tenantScoped == null)
            {
                return false;
            }

            var tenant = ApplicationContext.User as TenantPrincipal;
            if (tenant == null)
            {
                throw new InternalException("No tenant principal in context");
            }

            this.Set(@event.Persister, @event.State, "TenantId", tenant.TenantId);

            tenantScoped.SetTenantId(tenant.TenantId);

            return false;
        }

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            int index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
            {
                return;
            }

            state[index] = value;
        }

        public static void AppendTo(Configuration configuration)
        {
            configuration.SetListeners(
                ListenerType.PreInsert,
                configuration.EventListeners.PreInsertEventListeners.Concat(new IPreInsertEventListener[] { new TenantEventListener() }).ToArray());

            configuration.SetListeners(
                ListenerType.PreUpdate,
                configuration.EventListeners.PreUpdateEventListeners.Concat(new IPreUpdateEventListener[] { new TenantEventListener() }).ToArray());
        }
    }
}