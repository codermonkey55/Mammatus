﻿using Mammatus.Library.Reflection.Common;
using Mammatus.Library.Reflection.Extensions.Core;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Mammatus.Library.Reflection.Emitter
{
    internal class MemberGetEmitter : BaseEmitter
    {
        public MemberGetEmitter(MemberInfo memberInfo, Flags bindingFlags)
            : this(memberInfo.DeclaringType, bindingFlags, memberInfo.MemberType, memberInfo.Name, memberInfo)
        {
        }

        public MemberGetEmitter(Type targetType, Flags bindingFlags, MemberTypes memberType, string fieldOrPropertyName)
            : this(targetType, bindingFlags, memberType, fieldOrPropertyName, null)
        {
        }

        private MemberGetEmitter(Type targetType, Flags bindingFlags, MemberTypes memberType, string fieldOrPropertyName, MemberInfo memberInfo)
            : base(new CallInfo(targetType, null, bindingFlags, memberType, fieldOrPropertyName, Type.EmptyTypes, memberInfo, true))
        {
        }
        internal MemberGetEmitter(CallInfo callInfo) : base(callInfo)
        {
        }

        protected internal override DynamicMethod CreateDynamicMethod()
        {
            return CreateDynamicMethod("getter", CallInfo.TargetType, Common.Constants.ObjectType, new[] { Common.Constants.ObjectType });
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

            if (handleInnerStruct)
            {
                Generator.ldarg_0                               // load arg-0 (this)
                         .DeclareLocal(CallInfo.TargetType);    // TargetType tmpStr
                LoadInnerStructToLocal(0);                      // tmpStr = ((ValueTypeHolder)this)).Value
                Generator.DeclareLocal(Common.Constants.ObjectType);   // object result;
            }
            else if (!CallInfo.IsStatic)
            {
                Generator.ldarg_0                               // load arg-0 (this)
                         .castclass(CallInfo.TargetType);     // (TargetType)this
            }

            if (member.MemberType == MemberTypes.Field)
            {
                var field = member as FieldInfo;

                if (field.DeclaringType.IsEnum) // special enum handling as ldsfld does not support enums
                {
                    Generator.ldc_i4((int)field.GetValue(field.DeclaringType))
                             .boxIfValueType(field.FieldType);
                }
                else
                {
                    Generator.ldfld(field.IsStatic, field)        // (this|tmpStr).field OR TargetType.field
                             .boxIfValueType(field.FieldType);    // (object)<stack>
                }
            }
            else
            {
                var prop = member as PropertyInfo;
                MethodInfo getMethod = LookupUtils.GetPropertyGetMethod(prop, CallInfo);
                Generator.call(getMethod.IsStatic || CallInfo.IsTargetTypeStruct, getMethod) // (this|tmpStr).prop OR TargetType.prop
                         .boxIfValueType(prop.PropertyType);    // (object)<stack>
            }

            if (handleInnerStruct)
            {
                Generator.stloc_1.end();        // resultLocal = <stack>
                StoreLocalToInnerStruct(0);     // ((ValueTypeHolder)this)).Value = tmpStr
                Generator.ldloc_1.end();        // push resultLocal
            }

            Generator.ret();

            return Method.CreateDelegate(typeof(MemberGetter));
        }
    }
}