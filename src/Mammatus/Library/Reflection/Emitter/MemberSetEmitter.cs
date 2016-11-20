using Mammatus.Library.Reflection.Common;
using Mammatus.Library.Reflection.Extensions.Core;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Mammatus.Library.Reflection.Emitter
{
    internal class MemberSetEmitter : BaseEmitter
    {
        public MemberSetEmitter(MemberInfo memberInfo, Flags bindingFlags)
            : this(memberInfo.DeclaringType, bindingFlags, memberInfo.MemberType, memberInfo.Name, memberInfo)
        {
        }

        public MemberSetEmitter(Type targetType, Flags bindingFlags, MemberTypes memberType, string fieldOrProperty)
            : this(targetType, bindingFlags, memberType, fieldOrProperty, null)
        {
        }

        private MemberSetEmitter(Type targetType, Flags bindingFlags, MemberTypes memberType, string fieldOrProperty, MemberInfo memberInfo)
            : base(new CallInfo(targetType, null, bindingFlags, memberType, fieldOrProperty, Common.Constants.ArrayOfObjectType, memberInfo, false))
        {
        }
        internal MemberSetEmitter(CallInfo callInfo) : base(callInfo)
        {
        }

        protected internal override DynamicMethod CreateDynamicMethod()
        {
            return CreateDynamicMethod("setter", CallInfo.TargetType, null, new[] { Common.Constants.ObjectType, Common.Constants.ObjectType });
        }

        protected internal override Delegate CreateDelegate()
        {
            MemberInfo member = CallInfo.MemberInfo;
            if (member == null)
            {
                member = LookupUtils.GetMember(CallInfo);
                CallInfo.IsStatic = member.IsStatic();
            }
            bool handleInnerStruct = CallInfo.ShouldHandleInnerStruct;

            if (CallInfo.IsStatic)
            {
                Generator.ldarg_1.end();                            // load value-to-be-set
            }
            else
            {
                Generator.ldarg_0.end();                            // load arg-0 (this)
                if (handleInnerStruct)
                {
                    Generator.DeclareLocal(CallInfo.TargetType);    // TargetType tmpStr
                    LoadInnerStructToLocal(0);                      // tmpStr = ((ValueTypeHolder)this)).Value;
                    Generator.ldarg_1.end();                        // load value-to-be-set;
                }
                else
                {
                    Generator.castclass(CallInfo.TargetType)      // (TargetType)this
                        .ldarg_1.end();                             // load value-to-be-set;
                }
            }

            Generator.CastFromObject(member.Type());                // unbox | cast value-to-be-set
            if (member.MemberType == MemberTypes.Field)
            {
                var field = member as FieldInfo;
                Generator.stfld(field.IsStatic, field);             // (this|tmpStr).field = value-to-be-set;
            }
            else
            {
                var prop = member as PropertyInfo;
                MethodInfo setMethod = LookupUtils.GetPropertySetMethod(prop, CallInfo);
                Generator.call(setMethod.IsStatic || CallInfo.IsTargetTypeStruct, setMethod); // (this|tmpStr).set_Prop(value-to-be-set);
            }

            if (handleInnerStruct)
            {
                StoreLocalToInnerStruct(0); // ((ValueTypeHolder)this)).Value = tmpStr
            }

            Generator.ret();

            return Method.CreateDelegate(typeof(MemberSetter));
        }
    }
}