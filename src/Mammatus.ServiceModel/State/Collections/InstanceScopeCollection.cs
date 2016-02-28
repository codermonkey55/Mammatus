using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.ServiceModel;
using Mammatus.Core.Application;

namespace Mammatus.ServiceModel.State.Collections
{
    internal class InstanceScopeCollection : ObjectBase<InstanceScopeCollection>, IExtension<InstanceContext>
    {
        Hashtable _state = new Hashtable();

        private InstanceScopeCollection()
        {

        }

        public void Add(string key, object instance)
        {
            lock (_state.SyncRoot)
                _state.Add(key, instance);
        }

        public object Get(string key)
        {
            lock (_state.SyncRoot)
                return _state[key];
        }

        public void Remove(string key)
        {
            lock (_state.SyncRoot)
                _state.Remove(key);
        }

        public void Clear()
        {
            lock (_state.SyncRoot)
                _state.Clear();
        }

        public void Attach(InstanceContext owner) { }

        public void Detach(InstanceContext owner)
        {
            _state.Clear();
            _state = null;
        }
    }
}