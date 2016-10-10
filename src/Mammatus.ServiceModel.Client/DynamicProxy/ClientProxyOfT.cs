using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    /// <summary>
    /// This ClientProxy should be used when interfacing with a WCF service
    /// interface. It will ensure that the proxy is always valid and that the
    /// channel used to communicate with the service is recreated when a fault
    /// occurs. Testing Purposes
    /// </summary>
    /// <typeparam name="TInterface">Defines the type of service contract for
    /// which this client proxy needs to be build.</typeparam>
    public class ClientProxy<TInterface> : ClientProxy where TInterface : class
    {
        #region Constructors

        /// <summary>
        /// This constructor is not public since it is not allowed to instantiate
        /// this class by a developer. The only valid way is by calling the
        /// GetInstance method.
        /// </summary>
        /// <param name="binding">The binding used to connect with the remote
        /// service.</param>
        /// <param name="endpointAddress">The address of the remote service.</param>
        protected ClientProxy()
        {
            _instantiationMode = ClientEndpointSetting.Default;

            // Create the actual client proxy based upon the passed binding and
            // address of the service.
            CreateClientProxy();
        }

        /// <summary>
        /// This constructor is not public since it is not allowed to instantiate
        /// this class by a developer. The only valid way is by calling the
        /// GetInstance method.
        /// </summary>
        /// <param name="binding">The binding used to connect with the remote
        /// service.</param>
        /// <param name="endpointAddress">The address of the remote service.</param>
        protected ClientProxy(String endpointConfigurationName)
        {
            _endpointConfigurationName = endpointConfigurationName;

            _instantiationMode = ClientEndpointSetting.ConfigurationName;

            // Create the actual client proxy based upon the passed binding and
            // address of the service.
            CreateClientProxy();
        }

        /// <summary>
        /// This constructor is not public since it is not allowed to instantiate
        /// this class by a developer. The only valid way is by calling the
        /// GetInstance method.
        /// </summary>
        /// <param name="binding">The binding used to connect with the remote
        /// service.</param>
        /// <param name="endpointAddress">The address of the remote service.</param>
        protected ClientProxy(Binding binding, EndpointAddress endpointAddress)
        {
            _binding = binding;

            _endpointAddress = endpointAddress;

            _instantiationMode = ClientEndpointSetting.BindingAndAddress;

            // Create the actual client proxy based upon the passed binding and
            // address of the service.
            CreateClientProxy();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Make the channel of the enclosed proxy available for the derived and
        /// generated proxy implementations. Check ProxyClassBuilder.GenerateMethodImplementation
        /// for more information regarding this channel property. Look for 'get_Channel'.
        /// </summary>
        protected TInterface Channel
        {
            get
            {
                return _clientProxy.InnerChannel as TInterface;
            }
        }

        /// <summary>
        /// Make the channel of the enclosed proxy available for the derived and
        /// generated proxy implementations. Check ProxyClassBuilder.GenerateMethodImplementation
        /// for more information regarding this channel property. Look for 'get_Channel'.
        /// </summary>
        public TInterface CurrentChannel
        {
            get
            {
                return _clientProxy.InnerChannel as TInterface;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Close the current client proxy.
        /// </summary>
        private void CloseClientProxy()
        {
            if (_clientProxy != null)
            {
                if (_clientProxy.InnerChannel.State != CommunicationState.Faulted)
                {
                    _clientProxy.Close();
                }
                else
                {
                    _clientProxy.Abort();
                }

                _clientProxy = null;
            }
        }

        /// <summary>
        /// Create the actual client proxy based upon the passed binding and
        /// address of the service.
        /// </summary>
        private void CreateClientProxy()
        {
            if (_clientProxy != null)
            {
                this.CloseClientProxy();
            }

            switch (_instantiationMode)
            {
                case ClientEndpointSetting.Default:
                    _clientProxy = new InternalClientProxy<TInterface>();
                    this._binding = _clientProxy.Endpoint.Binding;
                    this._endpointAddress = _clientProxy.Endpoint.Address;
                    this._endpointConfigurationName = _clientProxy.Endpoint.Name;
                    break;

                case ClientEndpointSetting.ConfigurationName:
                    _clientProxy = new InternalClientProxy<TInterface>(_endpointConfigurationName);
                    this._binding = _clientProxy.Endpoint.Binding;
                    this._endpointAddress = _clientProxy.Endpoint.Address;
                    break;

                case ClientEndpointSetting.BindingAndAddress:
                    _clientProxy = new InternalClientProxy<TInterface>(_binding, _endpointAddress);
                    this._endpointConfigurationName = _clientProxy.Endpoint.Name;
                    break;
            }

            if (ProxyCreated != null)
            {
                ProxyCreated(this, new ParameterEventArgs<ClientBase<TInterface>>(_clientProxy));
            }
        }

        /// <summary>
        /// Handle the passed exception which occurred while calling a
        /// method/property on the service.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        protected virtual void HandleException(Exception exception)
        {
            Debug.WriteLine("HandleException: " + exception.Message);

            // Recreate the client proxy.
            CreateClientProxy();

            // We need to rethrow the exception wrappen in our
            // ConnectionProblemException so that callers of the methods/properties
            // of the service know that this exception needs to be catched.
            if ((exception is EndpointNotFoundException) || (exception is CommunicationException))
            {
                throw new ConnectionProblemException("Server unreachable",
                    exception);
            }
            else
            {
                throw new ConnectionProblemException("Unknown exception", exception);
            }
        }

        /// <summary>
        /// Close the current client proxy.
        /// </summary>
        protected internal override void CloseConnectionForClientProxy()
        {
            if (isInternalCall)
                CloseClientProxy();
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is fired when the internal proxy has been created.
        /// </summary>
        public static event EventHandler<ParameterEventArgs<ClientBase<TInterface>>> ProxyCreated;

        #endregion

        #region Fields

        /// <summary>
        /// This is the "real" proxy used to interface with the WCF service.
        /// However when an error occurs with the connection this proxy can no
        /// longer be used and it should be recreated.
        /// </summary>
        private InternalClientProxy<TInterface> _clientProxy = null;

        /// <summary>
        /// The binding that should be used to communicate with the remote WCF
        /// service.
        /// </summary>
        private Binding _binding = null;

        /// <summary>
        /// The end point address of the WCF service.
        /// </summary>
        private EndpointAddress _endpointAddress = null;

        /// <summary>
        ///
        /// </summary>
        private String _endpointConfigurationName = String.Empty;

        /// <summary>
        ///
        /// </summary>
        private ClientEndpointSetting _instantiationMode = ClientEndpointSetting.None;

        #endregion
    }
}
