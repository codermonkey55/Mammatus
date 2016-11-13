using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;

namespace Mammatus.Data.NHibernate.Database
{
    public interface ISessionManager
    {

    }

    public class SessionManager : ISessionManager
    {
        private static ISessionFactory _sessionFactory;

        private static readonly Dictionary<string, object> KeyedSessions;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeInternalSessionFactory();

                return _sessionFactory;
            }
        }


        static SessionManager()
        {
            KeyedSessions = new Dictionary<string, object>();
        }

        protected SessionManager(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }


        private static void InitializeInternalSessionFactory()
        {
            _sessionFactory = SessionFactoryBuilder.CreateSessionFactory();
        }


        public static ISession OpenSession(bool beginTransaction)
        {
            ISession session = SessionFactory.OpenSession();

            if (beginTransaction) session.BeginTransaction();

            return session;
        }

        public static ISession OpenSession(bool beginTransaction, IDbConnection dbconnection)
        {
            ISession session = SessionFactory.OpenSession(dbconnection);

            if (beginTransaction) session.BeginTransaction();

            return session;
        }


        public static INHExtendedSession GetDecoratedSession(bool beginTransaction)
        {
            INHExtendedSession extSession = new NHExtendedSession(SessionFactory.OpenSession());

            if (beginTransaction) extSession.Session.BeginTransaction();

            return extSession;
        }

        public static INHExtendedSession GetDecoratedSession(bool beginTransaction, IDbConnection dbconnection)
        {
            INHExtendedSession extSession = new NHExtendedSession(SessionFactory.OpenSession(dbconnection));

            if (beginTransaction) extSession.Session.BeginTransaction();

            return extSession;
        }


        public static string BindSession(object session)
        {
            string sessionKey = GenerateSessionKey();

            KeyedSessions.Add(sessionKey, session);

            return sessionKey;
        }

        public static TSession GetBoundedSession<TSession>(string sessionKey) where TSession : class
        {
            TSession session = null;

            if (KeyedSessions.ContainsKey(sessionKey))
            {
                session = KeyedSessions[sessionKey] as TSession;
            }

            return session;
        }


        private static string GenerateSessionKey()
        {
            Guid guid = Guid.NewGuid();

            string guidString = Convert.ToBase64String(guid.ToByteArray());

            guidString = guidString.Replace("=", "").Replace("+", "");

            return guidString;
        }
    }
}
