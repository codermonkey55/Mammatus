using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Mammatus.Library.Reflection.DynamicReflection
{
    internal sealed class DynamicBuilder : DynamicObject
    {
        private readonly Dictionary<string, object> members = new Dictionary<string, object>();

        #region DynamicObject Overrides

        /// <summary>
        /// Assigns the given value to the specified member, overwriting any previous definition if one existed.
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            members[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// Gets the value of the specified member.
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (members.ContainsKey(binder.Name))
            {
                result = members[binder.Name];
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        /// <summary>
        /// Invokes the specified member (if it is a delegate).
        /// </summary>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            object member;
            if (members.TryGetValue(binder.Name, out member))
            {
                var method = member as Delegate;
                if (method != null)
                {
                    result = method.DynamicInvoke(args);
                    return true;
                }
            }
            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// Gets a list of all dynamically defined members.
        /// </summary>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return members.Keys;
        }

        #endregion
    }
}
