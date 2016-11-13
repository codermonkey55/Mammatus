using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Event.Default;

namespace Mammatus.Data.NHibernate.Listeners
{
    /// <summary>
    /// Fix for issue: https://hibernate.onjira.com/browse/HHH-2763
    /// http://stackoverflow.com/questions/3090733/an-nhibernate-audit-trail-that-doesnt-cause-collection-was-not-processed-by-fl
    /// </summary>
    /// <param name="session">The session.</param>
    public class FixedDefaultFlushEventListener : DefaultFlushEventListener
    {
        /// <summary>
        /// Fix for issue: https://hibernate.onjira.com/browse/HHH-2763
        /// http://stackoverflow.com/questions/3090733/an-nhibernate-audit-trail-that-doesnt-cause-collection-was-not-processed-by-fl
        /// </summary>
        /// <param name="session">The session.</param>
        protected override void PerformExecutions(IEventSource session)
        {
            try
            {
                session.ConnectionManager.FlushBeginning();
                session.PersistenceContext.Flushing = true;
                session.ActionQueue.PrepareActions();
                session.ActionQueue.ExecuteActions();
            }
            finally
            {
                session.PersistenceContext.Flushing = false;
                session.ConnectionManager.FlushEnding();
            }
        }

        public static void OverrideIn(Configuration configuration)
        {
            configuration.SetListeners(
                ListenerType.Flush,
                new IFlushEventListener[] { new FixedDefaultFlushEventListener() });
        }
    }
}