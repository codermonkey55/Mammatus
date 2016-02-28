using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.ServiceModel;
using Mammatus.Core.Application;

namespace Mammatus.ServiceModel.State.Collections
{
    internal class OperationScopeCollection : ObjectBase<OperationScopeCollection>, IExtension<OperationContext>
    {
        Hashtable _state = new Hashtable();

        private OperationScopeCollection()
        {

        }

        public void Add(string key, object instance)
        {
            _state.Add(key, instance);
        }

        public object Get(string key)
        {
            return _state[key];
        }

        public void Remove(string key)
        {
            _state.Remove(key);
        }

        public void Attach(OperationContext owner) { }

        public void Detach(OperationContext owner)
        {
            _state.Clear();
            _state = null;
        }

        public void Clear()
        {
            _state.Clear();
        }
    }
}