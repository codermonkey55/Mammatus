using Mammatus.Data.Contracts;
using NHibernate;
using System;
using System.Collections.Generic;

namespace Mammatus.Data.NHibernate.Database
{
    public interface INHUnitOfWork<TINHRepository> : IUnitOfWork<TINHRepository> where TINHRepository : ISessionRepository
    {
        void StartNewSession();

        void StartChildSession();
    }

    public class NHUnitOfWork : INHUnitOfWork<INHRepository>
    {
        private ISessionFactory _sessionFactory;
        private ISession _currentSession;

        private readonly IDictionary<Type, INHRepository> _repositories;

        public ISession Session
        {
            get
            {
                return _currentSession ?? (_currentSession = _sessionFactory.OpenSession());
            }
        }

        public NHUnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

            _repositories = new Dictionary<Type, INHRepository>();
        }

        public INHRepository this[Type repoType]
        {
            get
            {
                INHRepository repo = NHRepository.Empty;

                if (_repositories.ContainsKey(repoType))
                    repo = _repositories[repoType];

                return repo;
            }
        }

        public void Enroll<TDerivedRepo>(TDerivedRepo repository)
            where TDerivedRepo : INHRepository
        {
            if (repository == null)
                throw new ArgumentNullException(typeof(TDerivedRepo).Name);

            var entityCollection = new NHEntityCollection(Session);

            ((ISessionRepository)repository).SetCollection(entityCollection);

            _repositories[typeof(TDerivedRepo)] = repository;
        }


        public void StartNewSession()
        {
            Commit();

            _currentSession = _sessionFactory.OpenSession();

            foreach (var repo in _repositories.Values)
            {
                var entityCollection = new NHEntityCollection(Session);

                ((ISessionRepository)repo).SetCollection(entityCollection);
            }
        }

        public void StartChildSession()
        {
            throw new NotImplementedException();
        }

        public int Commit()
        {
            //-> Save changes with the default options.
            try
            {
                Session.Flush();
                Session.Clear();
                Session.Dispose();
                _currentSession = null;
            }
            catch (System.Exception)
            {

                return 0;
            }

            return 1;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_currentSession != null)
                {
                    _currentSession.Dispose();

                    _currentSession = null;
                }

                if (_sessionFactory != null)
                {
                    _sessionFactory.Dispose();

                    _sessionFactory = null;
                }

                if (_repositories != null)
                {
                    _repositories.Clear();
                }
            }
        }
    }
}
