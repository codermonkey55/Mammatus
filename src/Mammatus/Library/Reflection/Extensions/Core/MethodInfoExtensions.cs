using Mammatus.Library.Reflection.Emitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mammatus.Library.Reflection
{
    /// <summary>
    /// Extension methods for inspecting, invoking and working with methods.
    /// </summary>
    public static class MethodInfoExtensions
    {
        #region Access
        /// <summary>
        /// Invokes the static method identified by <paramref name="methodInfo"/> with <paramref name="parameters"/>
        /// as arguments.  Leave <paramref name="parameters"/> empty if the method has no argument.
        /// </summary>
        /// <returns>The return value of the method.</returns>
        /// <remarks>If the method has no return type, <c>null</c> is returned.</remarks>
        public static object Call(this MethodInfo methodInfo, params object[] parameters)
        {
            return methodInfo.DelegateForCallMethod()(null, parameters);
        }

        /// <summary>
        /// Invokes the instance method identified by <paramref name="methodInfo"/> on the object
        /// <paramref name="obj"/> with <paramref name="parameters"/> as arguments.
        /// Leave <paramref name="parameters"/> empty if the method has no argument.
        /// </summary>
        /// <returns>The return value of the method.</returns>
        /// <remarks>If the method has no return type, <c>null</c> is returned.</remarks>
        public static object Call(this MethodInfo methodInfo, object obj, params object[] parameters)
        {
            return methodInfo.DelegateForCallMethod()(obj, parameters);
        }

        /// <summary>
        /// Creates a delegate which can invoke the instance method identified by <paramref name="methodInfo"/>.
        /// </summary>
        public static MethodInvoker DelegateForCallMethod(this MethodInfo methodInfo)
        {
            var flags = methodInfo.IsStatic ? Flags.StaticAnyVisibility : Flags.InstanceAnyVisibility;
            return (MethodInvoker)new MethodInvocationEmitter(methodInfo, flags).GetDelegate();
        }
        #endregion

        #region Method Parameter Lookup
        /// <summary>
        /// Gets all parameters for the given <paramref name="method"/>.
        /// </summary>
        /// <returns>The list of parameters for the method. This value will never be null.</returns>
        public static IList<ParameterInfo> Parameters(this MethodBase method)
        {
            return method.GetParameters();
        }
        #endregion

        #region Method Signature Comparer
        /// <summary>
        /// Compares the signature of the method with the given parameter types and returns true if
        /// all method parameters have the same order and type. Parameter names are not considered.
        /// </summary>
        /// <returns>True if the supplied parameter type array matches the method parameters array, false otherwise.</returns>
        public static bool HasParameterSignature(this MethodBase method, Type[] parameters)
        {
            return method.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters);
        }

        /// <summary>
        /// Compares the signature of the method with the given parameter types and returns true if
        /// all method parameters have the same order and type. Parameter names are not considered.
        /// </summary>
        /// <returns>True if the supplied parameter type array matches the method parameters array, false otherwise.</returns>
        public static bool HasParameterSignature(this MethodBase method, ParameterInfo[] parameters)
        {
            return method.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameters.Select(p => p.ParameterType));
        }
        #endregion
    }
}