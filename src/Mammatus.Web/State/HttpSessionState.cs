using System;
using Mammatus.Core.State;
using Mammatus.Helpers;

namespace Mammatus.Web.State
{
    public class HttpSessionState : ISessionState
    {
        private readonly IHttpContextProvider _contextProvider;

        public HttpSessionState(IHttpContextProvider context)
        {
            _contextProvider = context;
        }

        public string SessionId
        {
            get
            {
                return _contextProvider.HttpContext.Session.SessionID;
            }
        }

        public bool TryGet<T>(string sessionKey, out T value)
        {
            bool hasValue = false;
            value = default(T);
            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new ArgumentNullException("sessionKey");
            }

            object sessionItem = _contextProvider.HttpContext.Session[sessionKey];
            if (sessionItem != null)
            {
                hasValue = true;
                value = (T)sessionItem;
            }
            return hasValue;
        }

        public T Get<T>()
        {
            return Get<T>(null);
        }

        public T Get<T>(object key)
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>(key);

            lock (_contextProvider.HttpContext.Session.SyncRoot)
                return (T)_contextProvider.HttpContext.Session[fullKey];
        }

        public void Put<T>(T instance)
        {
            Put(default(object), instance);
        }

        public void Put<T>(object key, T instance)
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>(key);

            lock (_contextProvider.HttpContext.Session.SyncRoot)
                _contextProvider.HttpContext.Session[fullKey] = instance;
        }

        public void Remove<T>()
        {
            Remove<T>(null);
        }

        public void Remove<T>(object key)
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>(key);

            lock (_contextProvider.HttpContext.Session.SyncRoot)
                _contextProvider.HttpContext.Session.Remove(fullKey);
        }

        public void Clear()
        {
            _contextProvider.HttpContext.Session.Clear();
        }



        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGet<T>(object key, out T value)
        {
            throw new NotImplementedException();
        }

        public void Store(string key, object value)
        {
            throw new NotImplementedException();
        }
    }
}