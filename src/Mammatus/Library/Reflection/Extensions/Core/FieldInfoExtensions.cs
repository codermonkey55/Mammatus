using Mammatus.Library.Reflection.Emitter;
using System.Reflection;

namespace Mammatus.Library.Reflection
{
    /// <summary>
    /// Extension methods for inspecting and working with fields.
    /// </summary>
    public static class FieldInfoExtensions
    {
        /// <summary>
        /// Sets the static field identified by <paramref name="fieldInfo"/> to the specified <paramref name="value" />.
        /// </summary>
        public static void Set(this FieldInfo fieldInfo, object value)
        {
            fieldInfo.DelegateForSetFieldValue()(null, value);
        }

        /// <summary>
        /// Sets the instance field identified by <paramref name="fieldInfo"/> on the given <paramref name="obj"/>
        /// to the specified <paramref name="value" />.
        /// </summary>
        public static void Set(this FieldInfo fieldInfo, object obj, object value)
        {
            fieldInfo.DelegateForSetFieldValue()(obj, value);
        }

        /// <summary>
        /// Gets the value of the static field identified by <paramref name="fieldInfo"/>.
        /// </summary>
        public static object Get(this FieldInfo fieldInfo)
        {
            return fieldInfo.DelegateForGetFieldValue()(null);
        }

        /// <summary>
        /// Gets the value of the instance field identified by <paramref name="fieldInfo"/> on the given <paramref name="obj"/>.
        /// </summary>
        public static object Get(this FieldInfo fieldInfo, object obj)
        {
            return fieldInfo.DelegateForGetFieldValue()(obj);
        }

        /// <summary>
        /// Creates a delegate which can set the value of the field identified by <paramref name="fieldInfo"/>.
        /// </summary>
        public static MemberSetter DelegateForSetFieldValue(this FieldInfo fieldInfo)
        {
            var flags = fieldInfo.IsStatic ? Flags.StaticAnyVisibility : Flags.InstanceAnyVisibility;
            return (MemberSetter)new MemberSetEmitter(fieldInfo, flags).GetDelegate();
        }

        /// <summary>
        /// Creates a delegate which can get the value of the field identified by <paramref name="fieldInfo"/>.
        /// </summary>
        public static MemberGetter DelegateForGetFieldValue(this FieldInfo fieldInfo)
        {
            var flags = fieldInfo.IsStatic ? Flags.StaticAnyVisibility : Flags.InstanceAnyVisibility;
            return (MemberGetter)new MemberGetEmitter(fieldInfo, flags).GetDelegate();
        }
    }
}