using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Mammatus.Library.Reflection
{
    internal sealed class DynamicWrapper : DynamicObject
    {
        private readonly object target;

        #region Constructors
        public DynamicWrapper(object target)
        {
            this.target = target;
        }
        public DynamicWrapper(ref ValueType target)
        {
            this.target = target.WrapIfValueType();
        }
        #endregion

        #region DynamicObject Overrides
        /// <summary>
        /// Sets the member on the target to the given value. Returns true if the value was
        /// actually written to the underlying member.
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return target.TrySetValue(binder.Name, value);
        }

        /// <summary>
        /// Gets the member on the target and assigns it to the result parameter. Returns
        /// true if a value other than null was found and false otherwise.
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = target.TryGetValue(binder.Name);
            return result != null;
        }

        /// <summary>
        /// Invokes the method specified and assigns the result to the result parameter. Returns
        /// true if a method to invoke was found and false otherwise.
        /// </summary>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var bindingFlags = Flags.InstanceAnyVisibility | Flags.IgnoreParameterModifiers;
            var method = target.GetType().Method(binder.Name, args.ToTypeArray(), bindingFlags);
            result = method == null ? null : method.Call(target, args);
            return method != null;
        }

        /// <summary>
        /// Gets all member names from the underlying instance.
        /// </summary>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return target.GetType().Members().Select(m => m.Name);
        }
        #endregion
    }
}
