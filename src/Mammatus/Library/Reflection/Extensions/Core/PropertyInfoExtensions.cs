using Mammatus.Library.Reflection.Common;
using Mammatus.Library.Reflection.Emitter;
using System.Reflection;

namespace Mammatus.Library.Reflection.Extensions.Core
{
    /// <summary>
    /// Extension methods for inspecting and working with properties.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Sets the static property identified by <paramref name="propInfo"/> to the specified <paramref name="value" />.
        /// </summary>
        public static void Set(this PropertyInfo propInfo, object value)
        {
            propInfo.DelegateForSetPropertyValue(Flags.StaticAnyVisibility)(null, value);
        }

        /// <summary>
        /// Sets the instance property identified by <paramref name="propInfo"/> on the given <paramref name="obj"/>
        /// to the specified <paramref name="value" />.
        /// </summary>
        public static void Set(this PropertyInfo propInfo, object obj, object value)
        {
            propInfo.DelegateForSetPropertyValue(Flags.InstanceAnyVisibility)(obj, value);
        }

        /// <summary>
        /// Gets the value of the static property identified by <paramref name="propInfo"/>.
        /// </summary>
        public static object Get(this PropertyInfo propInfo)
        {
            return propInfo.DelegateForGetPropertyValue(Flags.StaticAnyVisibility)(null);
        }

        /// <summary>
        /// Gets the value of the instance property identified by <paramref name="propInfo"/> on the given <paramref name="obj"/>.
        /// </summary>
        public static object Get(this PropertyInfo propInfo, object obj)
        {
            return propInfo.DelegateForGetPropertyValue(Flags.InstanceAnyVisibility)(obj);
        }

        /// <summary>
        /// Creates a delegate which can set the value of the property <paramref name="propInfo"/>.
        /// </summary>
		public static MemberSetter DelegateForSetPropertyValue(this PropertyInfo propInfo)
        {
            var flags = propInfo.IsStatic() ? Flags.StaticAnyVisibility : Flags.InstanceAnyVisibility;
            return (MemberSetter)new MemberSetEmitter(propInfo, flags).GetDelegate();
        }

        /// <summary>
        /// Creates a delegate which can set the value of the property <param name="propInfo"/> matching the
        /// specified <param name="bindingFlags" />.
        /// </summary>
        public static MemberSetter DelegateForSetPropertyValue(this PropertyInfo propInfo, Flags bindingFlags)
        {
            return (MemberSetter)new MemberSetEmitter(propInfo, bindingFlags).GetDelegate();
        }

        /// <summary>
        /// Creates a delegate which can get the value of the property <param name="propInfo"/>.
        /// </summary>
		public static MemberGetter DelegateForGetPropertyValue(this PropertyInfo propInfo)
        {
            var flags = propInfo.IsStatic() ? Flags.StaticAnyVisibility : Flags.InstanceAnyVisibility;
            return (MemberGetter)new MemberGetEmitter(propInfo, flags).GetDelegate();
        }

        /// <summary>
        /// Creates a delegate which can get the value of the property <param name="propInfo"/> matching the
        /// specified <param name="bindingFlags" />.
        /// </summary>
        public static MemberGetter DelegateForGetPropertyValue(this PropertyInfo propInfo, Flags bindingFlags)
        {
            return (MemberGetter)new MemberGetEmitter(propInfo, bindingFlags).GetDelegate();
        }
    }
}