using System;
using Mammatus.Core.Application;
using Mammatus.Core.State;
using Mammatus.Helpers;
using Mammatus.ServiceModel.Runtime;
using Mammatus.ServiceModel.State.Collections;

namespace Mammatus.ServiceModel.State
{
    public class OperationScopeState : ObjectBase<OperationScopeState>, IScopeState
    {
        readonly OperationScopeCollection _collection;

        public OperationScopeState(IOperationContextProvider context)
        {
            _collection = context.OperationContext.Extensions.Find<OperationScopeCollection>();
            if (_collection == null)
            {
                _collection = OperationScopeCollection.Create();
                context.OperationContext.Extensions.Add(_collection);
            }
        }

        public T Get<T>()
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>();
            return (T)_collection.Get(fullKey);
        }

        public T Get<T>(object key)
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>(key);
            return (T)_collection.Get(fullKey);
        }

        public void Put<T>(T instance)
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>();
            _collection.Add(fullKey, instance);
        }

        public void Put<T>(object key, T instance)
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>(key);
            _collection.Add(fullKey, instance);
        }

        public void Remove<T>()
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>();
            _collection.Remove(fullKey);
        }

        public void Remove(string key)
        {
            _collection.Remove(key);
        }

        public void Remove<T>(object key)
        {
            var fullKey = NameKeyGenerator.BuildFullKey<T>(key);
            _collection.Remove(fullKey);
        }

        public void Clear()
        {
            _collection.Clear();
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