using NHibernate;
using System;
using System.Collections.Generic;

namespace Mammatus.Data.NHibernate.Database
{
    public interface INHExtendedSession
    {
        ISet<object> Items { get; }

        ISession Session { get; }

        ITransaction Transaction { get; }
    }

    public class NHExtendedSession : INHExtendedSession, IDisposable
    {
        protected readonly ISession InternalSession;

        protected readonly ISet<object> InternalItems;

        public NHExtendedSession(ISession session)
        {
            InternalSession = session;

            InternalItems = new HashSet<object>();
        }

        public ISet<object> Items
        {
            get { return InternalItems; }
        }

        public ISession Session
        {
            get { return InternalSession; }
        }

        public ITransaction Transaction
        {
            get { return InternalSession.Transaction; }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (InternalSession != null)
                    InternalSession.Dispose();

                if (InternalItems != null)
                    InternalItems.GetEnumerator().Dispose();
            }
        }
    }
}
