using System;
using System.Reflection;

namespace Mammatus.Library.Reflection.Emitter
{
    internal abstract class InvocationEmitter : BaseEmitter
    {
        protected InvocationEmitter(CallInfo callInfo)
            : base(callInfo)
        {
        }

        protected byte CreateLocalsForByRefParams(byte paramArrayIndex, MethodBase invocationInfo)
        {
            var paramTypes = CallInfo.MethodParamTypes ?? CallInfo.ParamTypes;
            byte numberOfByRefParams = 0;
            var parameters = invocationInfo.GetParameters();
            for (int i = 0; i < paramTypes.Length; i++)
            {
                Type paramType = paramTypes[i];
                if (paramType.IsByRef)
                {
                    Type type = paramType.GetElementType();
                    Generator.DeclareLocal(type);
                    if (!parameters[i].IsOut) // no initialization necessary is 'out' parameter
                    {
                        Generator.ldarg(paramArrayIndex)
                            .ldc_i4(i)
                            .ldelem_ref
                            .CastFromObject(type)
                            .stloc(numberOfByRefParams)
                            .end();
                    }
                    numberOfByRefParams++;
                }
            }
            return numberOfByRefParams;
        }

        protected void AssignByRefParamsToArray(int paramArrayIndex)
        {
            var paramTypes = CallInfo.MethodParamTypes ?? CallInfo.ParamTypes;
            byte currentByRefParam = 0;
            for (int i = 0; i < paramTypes.Length; i++)
            {
                Type paramType = paramTypes[i];
                if (paramType.IsByRef)
                {
                    Generator.ldarg(paramArrayIndex)
                        .ldc_i4(i)
                        .ldloc(currentByRefParam++)
                        .boxIfValueType(paramType.GetElementType())
                        .stelem_ref
                        .end();
                }
            }
        }

        protected void PushParamsOrLocalsToStack(int paramArrayIndex)
        {
            var paramTypes = CallInfo.MethodParamTypes ?? CallInfo.ParamTypes;
            byte currentByRefParam = 0;
            for (int i = 0; i < paramTypes.Length; i++)
            {
                Type paramType = paramTypes[i];
                if (paramType.IsByRef)
                {
                    Generator.ldloca_s(currentByRefParam++);
                }
                else
                {
                    Generator.ldarg(paramArrayIndex)
                        .ldc_i4(i)
                        .ldelem_ref
                        .CastFromObject(paramType);
                }
            }
        }
    }
}