using Mammatus.Data.Contracts;
using System.Data.Entity;

namespace Mammatus.Data.Entity
{
    public interface IEFRepository : ISessionRepository
    {

    }

    public abstract class EFRepository : IEFRepository, IPersistableRepository
    {
        private IEFCollectionPersister _persister;

        public static EmptyEFRepository Empty { get { return new EmptyEFRepository(); } }

        protected EFRepository(IEntityCollection entityCollection)
        {
            _persister = (IEFCollectionPersister)entityCollection;
        }

        public virtual void SetCollection(IEntityCollection entityCollection)
        {
            _persister = (IEFCollectionPersister)entityCollection;
        }

        public virtual void Modify(object entity)
        {
            if (_persister != null)
            {
                _persister.Attach(entity, EntityState.Modified);
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

    public sealed class EmptyEFRepository : EFRepository
    {
        internal EmptyEFRepository() : base(null)
        {

        }
    }
}
