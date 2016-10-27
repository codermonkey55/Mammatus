using NHibernate.Event;

namespace Mammatus.Data.NHibernate.Listeners
{
    internal sealed class FlushEntityAuditEventListener : IFlushEntityEventListener
    {
        public void OnFlushEntity(FlushEntityEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}