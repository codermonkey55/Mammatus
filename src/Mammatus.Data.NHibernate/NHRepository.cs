using PocoLib.Data.ControllerModel;

namespace PocoLib.Integration.NHibernate
{
    public interface INHRepository : ISessionRepository
    {

    }

    public abstract class NHRepository : INHRepository, IPersistableRepository
    {
        private INHCollectionPersister _persister;

        public static EmptyNHRepository Empty { get { return new EmptyNHRepository(); } }

        protected NHRepository(IEntityCollection entityCollection)
        {
            _persister = (INHCollectionPersister)entityCollection;
        }

        public virtual void SetCollection(IEntityCollection entityCollection)
        {
            _persister = (INHCollectionPersister)entityCollection;
        }

        public virtual void Modify(object entity)
        {
            if (_persister != null)
            {
                _persister.Merge(entity);
            }
        }

        int IPersistableRepository.Persist()
        {
            if (_persister != null)
            {
                return _persister.Persist();
            }

            return 0;
        }
    }

    public sealed class EmptyNHRepository : NHRepository
    {
        internal EmptyNHRepository() : base(null)
        {

        }
    }
}
