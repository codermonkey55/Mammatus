using Mammatus.Library.Reflection.Common;
using Mammatus.Library.Reflection.Emitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mammatus.Library.Reflection.Extensions.Core
{
    /// <summary>
    /// Extension methods for locating, inspecting and invoking constructors.
    /// </summary>
    public static class ConstructorExtensions
    {
        #region Constructor Invocation (CreateInstance)
        /// <summary>
        /// Invokes a constructor whose parameter types are inferred from <paramref name="parameters" />
        /// on the given <paramref name="type"/> with <paramref name="parameters" /> being the arguments.
        /// Leave <paramref name="parameters"/> empty if the constructor has no argument.
        /// </summary>
        /// <remarks>
        /// All elements of <paramref name="parameters"/> must not be <c>null</c>.  Otherwise,
        /// <see cref="NullReferenceException"/> is thrown.  If you are not sure as to whether
        /// any element is <c>null</c> or not, use the overload that accepts <c>paramTypes</c> array.
        /// </remarks>
        /// <seealso cref="CreateInstance(Type, Type[], object[])"/>
        public static object CreateInstance(this Type type, params object[] parameters)
        {
            return DelegateForCreateInstance(type, parameters.ToTypeArray())(parameters);
        }

        /// <summary>
        /// Invokes a constructor having parameter types specified by <paramref name="parameterTypes" />
        /// on the the given <paramref name="type"/> with <paramref name="parameters" /> being the arguments.
        /// </summary>
        public static object CreateInstance(this Type type, Type[] parameterTypes, params object[] parameters)
        {
            return DelegateForCreateInstance(type, parameterTypes)(parameters);
        }

        /// <summary>
        /// Invokes a constructor whose parameter types are inferred from <paramref name="parameters" /> and
        /// matching <paramref name="bindingFlags"/> on the given <paramref name="type"/>
        /// with <paramref name="parameters" /> being the arguments.
        /// Leave <paramref name="parameters"/> empty if the constructor has no argument.
        /// </summary>
        /// <remarks>
        /// All elements of <paramref name="parameters"/> must not be <c>null</c>.  Otherwise,
        /// <see cref="NullReferenceException"/> is thrown.  If you are not sure as to whether
        /// any element is <c>null</c> or not, use the overload that accepts <c>paramTypes</c> array.
        /// </remarks>
        /// <seealso cref="CreateInstance(System.Type,System.Type[],Flags,object[])"/>
        public static object CreateInstance(this Type type, Flags bindingFlags, params object[] parameters)
        {
            return DelegateForCreateInstance(type, bindingFlags, parameters.ToTypeArray())(parameters);
        }

        /// <summary>
        /// Invokes a constructor whose parameter types are <paramref name="parameterTypes" /> and
        /// matching <paramref name="bindingFlags"/> on the given <paramref name="type"/>
        /// with <paramref name="parameters" /> being the arguments.
        /// </summary>
        public static object CreateInstance(this Type type, Type[] parameterTypes, Flags bindingFlags, params object[] parameters)
        {
            return DelegateForCreateInstance(type, bindingFlags, parameterTypes)(parameters);
        }

        /// <summary>
        /// Creates a delegate which can invoke the constructor whose parameter types are <paramref name="parameterTypes" />
        /// on the given <paramref name="type"/>.  Leave <paramref name="parameterTypes"/> empty if the constructor
        /// has no argument.
        /// </summary>
        public static ConstructorInvoker DelegateForCreateInstance(this Type type, params Type[] parameterTypes)
        {
            return DelegateForCreateInstance(type, Flags.InstanceAnyVisibility, parameterTypes);
        }

        /// <summary>
        /// Creates a delegate which can invoke the constructor whose parameter types are <paramref name="parameterTypes" />
        /// and matching <paramref name="bindingFlags"/> on the given <paramref name="type"/>.
        /// Leave <paramref name="parameterTypes"/> empty if the constructor has no argument.
        /// </summary>
        public static ConstructorInvoker DelegateForCreateInstance(this Type type, Flags bindingFlags,
                                                                    params Type[] parameterTypes)
        {
            return (ConstructorInvoker)new CtorInvocationEmitter(type, bindingFlags, parameterTypes).GetDelegate();
        }
        #endregion

        #region Constructor Invocation (CreateInstances)
        /// <summary>
        /// Finds all types implementing a specific interface or base class <typeparamref name="T"/> in the
        /// given <paramref name="assembly"/> and invokes the default constructor on each to return a list of
        /// instances. Any type that is not a class or does not have a default constructor is ignored.
        /// </summary>
        /// <typeparam name="T">The interface or base class type to look for in the given assembly.</typeparam>
        /// <param name="assembly">The assembly in which to look for types derived from the type parameter.</param>
        /// <returns>A list containing one instance for every unique type implementing T. This will never be null.</returns>
        public static IList<T> CreateInstances<T>(this Assembly assembly)
        {
            var query = from type in assembly.TypesImplementing<T>()
                        where type.IsClass && !type.IsAbstract && type.Constructor() != null
                        select (T)type.CreateInstance();
            return query.ToList();
        }
        #endregion

        #region Constructor Lookup (Single)
        /// <summary>
        /// Gets the constructor corresponding to the supplied <paramref name="parameterTypes"/> on the
        /// given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to reflect on.</param>
        /// <param name="parameterTypes">The types of the constructor parameters in order.</param>
        /// <returns>The matching constructor or null if no match was found.</returns>
        public static ConstructorInfo Constructor(this Type type, params Type[] parameterTypes)
        {
            return type.Constructor(Flags.InstanceAnyVisibility, parameterTypes);
        }

        /// <summary>
        /// Gets the constructor matching the given <paramref name="bindingFlags"/> and corresponding to the
        /// supplied <paramref name="parameterTypes"/> on the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to reflect on.</param>
        /// <param name="bindingFlags">The search criteria to use when reflecting.</param>
        /// <param name="parameterTypes">The types of the constructor parameters in order.</param>
        /// <returns>The matching constructor or null if no match was found.</returns>
        public static ConstructorInfo Constructor(this Type type, Flags bindingFlags, params Type[] parameterTypes)
        {
            return type.GetConstructor(bindingFlags, null, parameterTypes, null);
        }
        #endregion

        #region Constructor Lookup (Multiple)
        /// <summary>
        /// Gets all public and non-public constructors (that are not abstract) on the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to reflect on.</param>
        /// <returns>A list of matching constructors. This value will never be null.</returns>
        public static IList<ConstructorInfo> Constructors(this Type type)
        {
            return type.Constructors(Flags.InstanceAnyVisibility);
        }

        /// <summary>
        /// Gets all constructors matching the given <paramref name="bindingFlags"/> (and that are not abstract)
        /// on the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to reflect on.</param>
        /// <param name="bindingFlags">The search criteria to use when reflecting.</param>
        /// <returns>A list of matching constructors. This value will never be null.</returns>
        public static IList<ConstructorInfo> Constructors(this Type type, Flags bindingFlags)
        {
            return type.GetConstructors(bindingFlags); //.Where( ci => !ci.IsAbstract ).ToList();
        }
        #endregion
    }
}