using System;
using System.Diagnostics;
using System.Reflection;

namespace Mammatus.Library.Reflection.Emitter
{
    /// <summary>
    /// Stores all necessary information to construct a dynamic method for member mapping.
    /// </summary>
    [DebuggerStepThrough]
    internal class MapCallInfo : CallInfo
    {
        public Type SourceType { get; private set; }
        public MemberTypes SourceMemberTypes { get; private set; }
        public MemberTypes TargetMemberTypes { get; private set; }
        public string[] Names { get; private set; }

        public MapCallInfo(Type targetType, Type[] genericTypes, Flags bindingFlags, MemberTypes memberTypes, string name, Type[] parameterTypes, MemberInfo memberInfo, bool isReadOperation, Type sourceType, MemberTypes sourceMemberTypes, MemberTypes targetMemberTypes, string[] names) : base(targetType, genericTypes, bindingFlags, memberTypes, name, parameterTypes, memberInfo, isReadOperation)
        {
            SourceType = sourceType;
            SourceMemberTypes = sourceMemberTypes;
            TargetMemberTypes = targetMemberTypes;
            Names = names;
        }

        public override bool Equals(object obj)
        {
            var other = obj as MapCallInfo;
            if (other == null)
            {
                return false;
            }
            if (!base.Equals(obj))
            {
                return false;
            }
            if (other.SourceType != SourceType ||
                other.SourceMemberTypes != SourceMemberTypes ||
                other.TargetMemberTypes != TargetMemberTypes ||
                (other.Names == null && Names != null) ||
                (other.Names != null && Names == null) ||
                (other.Names != null && Names != null && other.Names.Length != Names.Length))
            {
                return false;
            }
            if (other.Names != null && Names != null)
            {
                for (int i = 0; i < Names.Length; i++)
                {
                    if (Names[i] != other.Names[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = base.GetHashCode() + SourceType.GetHashCode() * SourceMemberTypes.GetHashCode() * TargetMemberTypes.GetHashCode();
            for (int i = 0; i < Names.Length; i++)
            {
                hash += Names[i].GetHashCode() * (i + 1);
            }
            return hash;
        }
    }
}