using Mammatus.Data.Contracts;
using NHibernate;

namespace Mammatus.Data.NHibernate
{
    public abstract class NHDomainRepository : IPersistableRepository
    {
        protected readonly ISession _session;

        public NHDomainRepository(ISession session)
        {
            _session = session;
        }

        public void Modify(object entity)
        {
            _session.Update(entity);
        }

        public int Persist()
        {
            try
            {
                _session.Flush();
                _session.Clear();
            }
            catch (System.Exception)
            {

                return 0;
            }

            return 1;
        }
    }
}
