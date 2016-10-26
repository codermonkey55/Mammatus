using Mammatus.Data.Contracts;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace Mammatus.Data.NHibernate
{
    public interface INHEntityCollection : IEntityCollection
    {
        IQueryOver<TEntity, TEntity> Collection<TEntity>() where TEntity : class;

        void Update(object entity);
    }

    public interface INHEntityCollection<T> : INHEntityCollection where T : class
    {
        IQueryOver<T, T> Entities { get; }

        IQueryable<T> Linq { get; }

        T Get(object id);

        void Update(T entity);

        T Remove(T entity);

        T Save(T entity);
    }

    public interface INHCollectionPersister : ICollectionPersister
    {
        void Merge(object entity);
    }

    public interface INHCollectionPersister<T> : ICollectionPersister where T : class
    {
        void Merge(T entity);
    }

    public class NHEntityCollection : INHEntityCollection, INHCollectionPersister
    {
        protected readonly ISession _session;
        protected readonly ISessionFactory _sessionFactory;

        public NHEntityCollection(ISession session)
        {
            _session = session;
        }

        public IQueryOver<TEntity, TEntity> Collection<TEntity>() where TEntity : class
        {
            return _session.QueryOver<TEntity>();
        }

        public void Merge(object entity)
        {
            _session.Merge(entity);
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

        public void Update(object entity)
        {
            _session.Update(entity);
        }
    }

    public class EFEntityCollection<T> : NHEntityCollection, INHEntityCollection<T>, INHCollectionPersister<T> where T : class
    {
        public EFEntityCollection(ISession session) : base(session)
        {

        }

        public IQueryOver<T, T> Entities
        {
            get
            {
                return _session.QueryOver<T>();
            }
        }

        public IQueryable<T> Linq
        {
            get
            {
                return _session.Query<T>();
            }
        }

        public T Get(object id)
        {
            return _session.Get<T>(id);
        }

        public T Save(T entity)
        {
            _session.Save(entity);

            return entity;
        }

        public T Remove(T entity)
        {
            _session.Delete(entity);

            return entity;
        }

        public void Update(T entity)
        {
            base.Update(entity);
        }

        public void Merge(T entity)
        {
            base.Merge(entity);
        }
    }
}