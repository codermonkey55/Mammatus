using System;
using System.Reflection;

namespace Mammatus.Library.Reflection.Probing
{
    internal class ConstructorMap : MethodMap
    {
        private ConstructorInvoker invoker;

        public ConstructorMap(ConstructorInfo constructor, string[] paramNames, Type[] parameterTypes,
                               object[] sampleParamValues, bool mustUseAllParameters)
            : base(constructor, paramNames, parameterTypes, sampleParamValues, mustUseAllParameters)
        {
        }

        #region UpdateMembers Private Helper Method
        private void UpdateMembers(object target, object[] row)
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (parameterReflectionMask[i])
                {
                    MemberInfo member = members[i];
                    if (member != null)
                    {
                        object value = parameterTypeConvertMask[i] ? TypeConverter.Get(member.Type(), row[i]) : row[i];
                        member.Set(target, value);
                    }
                }
            }
        }
        #endregion

        public override object Invoke(object[] row)
        {
            object[] methodParameters = isPerfectMatch ? row : PrepareParameters(row);
            object result = invoker.Invoke(methodParameters);
            if (!isPerfectMatch && AnySet(parameterReflectionMask))
                UpdateMembers(result, row);
            return result;
        }

        internal override void InitializeInvoker()
        {
            invoker = type.DelegateForCreateInstance(GetParamTypes());
        }
    }
}