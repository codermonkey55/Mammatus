using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    /// <summary>
    /// This class builder is responsible for building client proxies which 
    /// can be used to communicate with a service. They do not have to be recreated 
    /// when the communication channel is severed, this is done automatically. This 
    /// builder constructs an assembly with the necessary types by generating MSIL.
    /// (Microsoft Intermediate Language).
    /// 
    /// Based upon information found at http://www.acorns.com.au/blog/?p=86
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>    
    internal class ProxyClassBuilder
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        internal ProxyClassBuilder()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="saveAssembly">Pass true to ensure that an assembly is 
        /// saved. This is only used for checking with reflection to ensure 
        /// that the correct code is generated. This should not be used for 
        /// a "real-life' application.</param>
        internal ProxyClassBuilder(bool saveAssembly)
        {
            _saveAssembly = saveAssembly;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate a type based on the passed class name within the generated 
        /// assembly. When a type is generated it will be cached so that future 
        /// calls do not get the penalty for creating it.
        /// </summary>
        /// <param name="className">The class name for which a type needs to be 
        /// generated.</param>
        /// <returns></returns>
        internal Type GenerateType<TInterface>(string className) where TInterface : class
        {
            Type type = null;

            // We first need to check if we already created a type for the 
            // passed class.
            if (!_cachedTypes.TryGetValue(className, out type))
            {
                GenerateAssembly();

                if (_moduleBuilder != null)
                {
                    type = _moduleBuilder.GetType(className);

                    // This type is not yet generated, so let's do that.
                    if (type == null)
                    {
                        type = GenerateTypeImplementation<TInterface>(className);

                        if (type != null)
                        {
                            _cachedTypes.Add(className, type);

                            // The assembly just got updated, so we have to 
                            // save it.
                            if (_saveAssembly)
                            {
                                Save();
                            }
                        }
                    }
                }
            }

            return type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instantiationMode"></param>
        internal void SetClientEndpointSetting(ClientEndpointSetting instantiationMode)
        {
            this._instantiationMode = instantiationMode;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Generate the assembly that is used for containing the generated 
        /// proxy classes. 
        /// </summary>
        private void GenerateAssembly()
        {
            if (_assemblyBuilder == null)
            {
                AssemblyName assemblyName = new AssemblyName(PROXY_CLASS_ASSEMBLY_NAME);

                _assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName,
                    _saveAssembly ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);

                if (_saveAssembly)
                {
                    _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name,
                        PROXY_CLASS_ASSEMBLY_NAME + ".dll");
                }
                else
                {
                    _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name);
                }
            }
        }

        /// <summary>
        /// Generate the implementation for the type of the class identified 
        /// by the passed class name.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The generated type.</returns>
        private Type GenerateTypeImplementation<TInterface>(string className) where TInterface : class
        {
            Type generatedType = null;

            TypeBuilder typeBuilder = _moduleBuilder.DefineType(
                PROXY_CLASS_ASSEMBLY_NAME + "." + className,
                TypeAttributes.Public, typeof(ClientProxy<TInterface>));

            if (typeBuilder != null)
            {
                // Our generated type will implement the interface as passed 
                // via the generic principle.
                typeBuilder.AddInterfaceImplementation(typeof(TInterface));

                // We need to generate the constructor which enables the user(s) 
                // to define the binding and the address of the service.
                GenerateConstructor<TInterface>(typeBuilder);

                GenerateMethodImplementations<TInterface>(typeBuilder, typeof(TInterface));

                generatedType = typeBuilder.CreateType();
            }

            return generatedType;
        }

        /// <summary>
        /// Generate a constructor for the type we are currently generating. 
        /// </summary>
        /// <param name="typeBuilder">The builder that is used for generating 
        /// the new type definition.</param>
        private void GenerateConstructor<TInterface>(TypeBuilder typeBuilder) where TInterface : class
        {
            Type[] constructorParameters = null;

            // Define the constructor signature that will be used to create instance of generated type.

            switch (_instantiationMode)
            {
                case ClientEndpointSetting.Default:
                    constructorParameters = null;
                    break;
                case ClientEndpointSetting.ConfigurationName:
                    constructorParameters = new[] { typeof(String) };
                    break;
                case ClientEndpointSetting.BindingAndAddress:
                    constructorParameters = new[] { typeof(Binding), typeof(EndpointAddress) };
                    break;
            }

            ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.RTSpecialName,
                CallingConventions.Standard, constructorParameters);

            ILGenerator iLGenerator = constructorBuilder.GetILGenerator();

            switch (_instantiationMode)
            {
                case ClientEndpointSetting.Default:
                    // This.
                    iLGenerator.Emit(OpCodes.Ldarg_0);
                    break;
                case ClientEndpointSetting.ConfigurationName:
                    // This.
                    iLGenerator.Emit(OpCodes.Ldarg_0);
                    // Load the first parameter (EndpointConfiguration).
                    iLGenerator.Emit(OpCodes.Ldarg_1);
                    break;
                case ClientEndpointSetting.BindingAndAddress:
                    // This.
                    iLGenerator.Emit(OpCodes.Ldarg_0);
                    // Load the first parameter (Binding).
                    iLGenerator.Emit(OpCodes.Ldarg_1);
                    // Load the second parameter (EndpointAddress).
                    iLGenerator.Emit(OpCodes.Ldarg_2);
                    break;
            }

            // We will be calling the constructor from our base class. This 
            // generated class is deriving from the ClientProxy class.
            ConstructorInfo originalConstructor =
                typeof(ClientProxy<TInterface>).GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null, constructorParameters, null);

            // Call the constructor from the base class.
            iLGenerator.Emit(OpCodes.Call, originalConstructor);

            // This method is finished.
            iLGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Save the current state of the assembly into a dll file. This makes 
        /// only sense when the ProxyClassBuilder is initialized with saveAssembly 
        /// set to true.
        /// </summary>
        private void Save()
        {
            if (_saveAssembly)
            {
                _assemblyBuilder.Save(PROXY_CLASS_ASSEMBLY_NAME + ".dll");
            }
        }

        /// <summary>
        /// Get the method from the base class with the matching method name 
        /// and return it.
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        /// <returns>The method info of the matching method.</returns>
        /// <exception cref="InvalidOperationException">This exception is thrown 
        /// when no matching method could be found. This is a serious problem 
        /// since we are unable to generate a working proxy class.</exception>
        protected MethodInfo GetMethodFromBaseClass<TInterface>(string methodName) where TInterface : class
        {
            MethodInfo methodInfo = typeof(ClientProxy<TInterface>).GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.NonPublic |
                BindingFlags.Public | BindingFlags.GetProperty);

            if (methodInfo == null)
            {
                throw new InvalidOperationException(String.Format(
                    "The method {0} could not be found in {1}",
                    methodName, typeof(ClientProxy<TInterface>)));
            }

            return methodInfo;
        }

        /// <summary>
        /// Generate the method implementations in our generate type based upon 
        /// the passed type. This enables us to put an extra layer between the 
        /// user(s) and the actual proxy used to communicate with a service. The 
        /// extra layer is our ClientProxy that will extend every call to the 
        /// service with exception handling and reconnection behaviour.
        /// </summary>
        /// <param name="typeBuilder">The builder that is used to generate the 
        /// implementation for the type.</param>
        /// <param name="interfaceType">The interface for which we are generating 
        /// the methods</param>
        protected virtual void GenerateMethodImplementations<TInterface>(
            TypeBuilder typeBuilder, Type interfaceType) where TInterface : class
        {
            MethodInfo[] methods = interfaceType.GetMethods();

            // Let's construct the methods as defined in the interface type so 
            // that we are able to generate our own implementation.
            foreach (MethodInfo method in methods)
            {
                // We need the types of the parameters for this method.
                Type[] parameterTypes = GetTypeOfParameters(method.GetParameters());

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name,
                    DEFAULT_METHOD_ATTRIBUTES, method.ReturnType, parameterTypes);

                // Start building the method.
                methodBuilder.CreateMethodBody(null, 0);
                ILGenerator iLGenerator = methodBuilder.GetILGenerator();

                // Generate the actual implementation for this method.
                GenerateMethodImplementation<TInterface>(method, parameterTypes, iLGenerator);

                // Declare that we override the interface method.
                typeBuilder.DefineMethodOverride(methodBuilder, method);
            }

            // Generate the implementations for any inherited interfaces.
            Type[] inheritedInterfaces = interfaceType.GetInterfaces();

            foreach (Type inheritedInterface in inheritedInterfaces)
            {
                GenerateMethodImplementations<TInterface>(typeBuilder, inheritedInterface);
            }
        }

        /// <summary>
        /// Generate the implementation for the method. It will generate the 
        /// functionality to call the method on the channel as offered by the 
        /// baseclass. It will add exception handling so that any exceptions 
        /// that occur (for example when no valid connection with the service is 
        /// present) are handled by the base class. The base class can then 
        /// take action to try to reconnect with the service.
        /// </summary>
        /// <param name="method">The method for which the implementation needs 
        /// to be generated.</param>
        /// <param name="parameterTypes">The parameter types which are passed 
        /// onto this method.</param>
        /// <param name="iLGenerator">The generator which needs to be used to 
        /// generate the implementation for the method.</param>
        protected void GenerateMethodImplementation<TInterface>(MethodInfo method,
            Type[] parameterTypes, ILGenerator iLGenerator) where TInterface : class
        {
            if (IsMethodWithReturnValue(method))
            {
                // Declare a variable which will contain the return type.
                iLGenerator.DeclareLocal(method.ReturnType);
            }

            // Add the 'try {' to the generated method implementation.
            Label tryLabel = iLGenerator.BeginExceptionBlock();
            {
                // This.
                iLGenerator.Emit(OpCodes.Ldarg_0);

                // Get the Channel property from our base class. This enables 
                // us to call the 'real' implementation which will transfer the 
                // call onto the connected service.
                MethodInfo channelProperty = GetMethodFromBaseClass<TInterface>("get_Channel");
                // Get the channel: "base.Channel<TInterface>."
                iLGenerator.EmitCall(OpCodes.Call, channelProperty, null);

                // Prepare the parameters for the call.
                ParameterInfo[] parameters = method.GetParameters();

                for (int index = 0; index < parameterTypes.Length; index++)
                {
                    iLGenerator.Emit(OpCodes.Ldarg, (((short)index) + 1));
                }

                // Call the Channel via the interface.
                iLGenerator.Emit(OpCodes.Callvirt, method);

                if (IsMethodWithReturnValue(method))
                {
                    // returnValue = result of the function call.
                    iLGenerator.Emit(OpCodes.Stloc_0);
                }
            }
            // Add the 'catch (Exception)' to the generated method implementation.
            {
                iLGenerator.BeginCatchBlock(typeof(Exception));

                // Declare a local variable which will be used to store the 
                // received exception.
                int localVariableIndex = iLGenerator.DeclareLocal(typeof(Exception)).LocalIndex;

                // Get the exception from the stack.
                iLGenerator.Emit(OpCodes.Stloc_S, localVariableIndex);

                // This.
                iLGenerator.Emit(OpCodes.Ldarg_0);

                // Load the exception into the local variable.
                iLGenerator.Emit(OpCodes.Ldloc_S, localVariableIndex);

                // Let's call the method from our base class responsible for 
                // handling the exception. 
                MethodInfo handleExceptionMethod =
                    GetMethodFromBaseClass<TInterface>("HandleException");

                // Let's call the virtual method.
                iLGenerator.Emit(OpCodes.Callvirt, handleExceptionMethod);
            }

            // Let's end the try/catch block.
            iLGenerator.EndExceptionBlock();

            if (IsMethodWithReturnValue(method))
            {
                // Return the returnValue.
                iLGenerator.Emit(OpCodes.Ldloc_0);
            }

            // This method is finished.
            iLGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Get the types of the parameters passed.
        /// </summary>
        /// <param name="parameters">The parameters for which we should return 
        /// the types.</param>
        /// <returns>The types for the passed parameters</returns>
        private static Type[] GetTypeOfParameters(ParameterInfo[] parameters)
        {
            List<Type> typeOfParameters = new List<Type>();

            foreach (ParameterInfo parameter in parameters)
            {
                typeOfParameters.Add(parameter.ParameterType);
            }

            return typeOfParameters.ToArray();
        }

        /// <summary>
        /// This checks if the passed method returns a value or not.
        /// </summary>
        /// <param name="methodInfo">The method that needs to be checked.</param>
        /// <returns>True when the method returns a value, false when it is a 
        /// void method.</returns>
        private bool IsMethodWithReturnValue(MethodInfo methodInfo)
        {
            return (methodInfo.ReturnType != typeof(void));
        }

        #endregion

        #region Fields

        /// <summary>
        /// This is used to store the generated types in so that future retrievals 
        /// can retrieve it from this cache.
        /// </summary>
        private Dictionary<string, Type> _cachedTypes =
            new Dictionary<string, Type>();

        /// <summary>
        /// The default attributes that we use for the generated methods.
        /// </summary>
        private const MethodAttributes DEFAULT_METHOD_ATTRIBUTES =
            MethodAttributes.Public | MethodAttributes.Virtual |
            MethodAttributes.Final | MethodAttributes.NewSlot;

        /// <summary>
        /// The builder that we use to generate the assembly which will contain 
        /// the generated proxy classes.
        /// </summary>
        private AssemblyBuilder _assemblyBuilder = null;

        /// <summary>
        /// The builder that we use to generate the module which will contain the 
        /// generated proxy classes.
        /// </summary>
        private ModuleBuilder _moduleBuilder = null;

        /// <summary>
        /// This is set to true when an assembly needs to be saved on disk with 
        /// the generated code for the client proxies. This is more for debugging 
        /// purposes then that it really serves a goal.
        /// </summary>
        private bool _saveAssembly = false;

        /// <summary>
        /// 
        /// </summary>
        private ClientEndpointSetting _instantiationMode = ClientEndpointSetting.None;

        /// <summary>
        /// The assembly name of the assembly that will contain the generated 
        /// proxy classes.
        /// </summary>
        private const string PROXY_CLASS_ASSEMBLY_NAME = "ProxyClassContainer";

        #endregion
    }
}
