using System;
using System.Collections;
using System.ServiceModel;
using Mammatus.Core.Application;
using Mammatus.Core.State;
using Mammatus.Helpers;
using Mammatus.ServiceModel.State.Collections;

namespace Mammatus.ServiceModel.State
{
    public class InstanceScopeState : ObjectBase<InstanceScopeState>, IScopeState
    {
        private readonly InstanceScopeCollection _collection;

        private InstanceScopeState(IOperationContextProvider context)
        {
            _collection = context.OperationContext.InstanceContext.Extensions.Find<InstanceScopeCollection>();

            if (_collection == null)
            {
                lock(context.OperationContext.InstanceContext)
                {
                    _collection = context.OperationContext.InstanceContext.Extensions.Find<InstanceScopeCollection>();

                    if (_collection == null)
                    {
                        _collection = InstanceScopeCollection.Create();
                        context.OperationContext.InstanceContext.Extensions.Add(_collection);
                    }
                }
            }
        }

        public T Get<T>()
        {
            return Get<T>(default(object));
        }

        public T Get<T>(object key)
        {
            return (T) _collection.Get(NameKeyGenerator.BuildFullKey<T>(key));
        }

        public void Put<T>(T instance)
        {
            Put(default(object), instance);
        }

        public void Put<T>(object key, T instance)
        {
            _collection.Add(NameKeyGenerator.BuildFullKey<T>(key), instance);
        }

        public void Remove<T>()
        {
            Remove<T>(default(object));
        }

        public void Remove<T>(object key)
        {
            _collection.Remove(NameKeyGenerator.BuildFullKey<T>(key));
        }

        public void Clear()
        {
            _collection.Clear();
        }



        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGet<T>(object Key, out T value)
        {
            throw new NotImplementedException();
        }

        public void Store(string key, object value)
        {
            throw new NotImplementedException();
        }
    }
}