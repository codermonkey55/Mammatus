using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyContainer
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ProxyContainer()
        {
            _classBuilder = new ProxyClassBuilder();

            _cachedClientProxies = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classBuilder"></param>
        /// <param name="cachedProxies"></param>
        public ProxyContainer(Dictionary<string, object> cachedProxies)
        {
            _classBuilder = new ProxyClassBuilder();

            _cachedClientProxies = cachedProxies;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get an instance of the client proxy with the correct identifier. 
        /// This identifier should be unique.
        /// </summary>
        /// <param name="identifier">Uniquely identifies the client proxy.</param>
        /// <param name="binding">The binding used to connect with the service.
        /// </param>
        /// <param name="endpointAddress">The address of the service.</param>
        /// <returns>The client proxy.</returns>
        public TInterface GetInstance<TInterface>(string identifier) where TInterface : class
        {
            TInterface interfaceInstance = null;

            Object generatedInstance = null;

            if (_cachedClientProxies.TryGetValue(identifier, out generatedInstance))
            {
                interfaceInstance = (TInterface)generatedInstance;
            }
            else
            {
                Type iType = typeof(TInterface);
                string typeIdentifier = iType.Name;

                // Remove the 'I' from the type name. 
                if (iType.Name.StartsWith("I") && char.IsUpper(iType.Name, 1))
                {
                    typeIdentifier = iType.Name.Substring(1);
                }

                _classBuilder.SetClientEndpointSetting(ClientEndpointSetting.Default);

                // The interface is not yet present in the cache, so let's 
                // create one and also add it to the cache.
                Type type = _classBuilder.GenerateType<TInterface>(typeIdentifier);

                // Create an instance of the proxy and construct it with the 
                // passed binding and endpoint address.
                var _generatedInstance = Activator.CreateInstance(type);

                interfaceInstance = (TInterface)_generatedInstance;

                ((ClientProxy<TInterface>)_generatedInstance)._identifier = identifier;

                _cachedClientProxies.Add(identifier, _generatedInstance);
            }

            return interfaceInstance as TInterface;
        }

        /// <summary>
        /// Get an instance of the client proxy with the correct identifier. 
        /// This identifier should be unique.
        /// </summary>
        /// <param name="identifier">Uniquely identifies the client proxy.</param>
        /// <param name="binding">The binding used to connect with the service.
        /// </param>
        /// <param name="endpointAddress">The address of the service.</param>
        /// <returns>The client proxy.</returns>
        public TInterface GetInstance<TInterface>(string identifier,
            Binding binding, EndpointAddress endpointAddress) where TInterface : class
        {
            TInterface interfaceInstance = null;

            Object generatedInstance = null;

            if (_cachedClientProxies.TryGetValue(identifier, out generatedInstance))
            {
                interfaceInstance = (TInterface)generatedInstance;
            }
            else
            {
                Type iType = typeof(TInterface);
                string typeIdentifier = iType.Name;

                // Remove the 'I' from the type name. 
                if (iType.Name.StartsWith("I") && char.IsUpper(iType.Name, 1))
                {
                    typeIdentifier = iType.Name.Substring(1);
                }

                _classBuilder.SetClientEndpointSetting(ClientEndpointSetting.BindingAndAddress);

                // The interface is not yet present in the cache, so let's 
                // create one and also add it to the cache.
                Type type = _classBuilder.GenerateType<TInterface>(typeIdentifier);


                // Create an instance of the proxy and construct it with the 
                // passed binding and endpoint address.
                var _generatedInstance = Activator.CreateInstance(type,
                    new object[] { binding, endpointAddress });

                interfaceInstance = (TInterface)_generatedInstance;

                ((ClientProxy<TInterface>)_generatedInstance)._identifier = identifier;

                _cachedClientProxies.Add(identifier, _generatedInstance);
            }

            return interfaceInstance as TInterface;
        }

        /// <summary>
        /// Get an instance of the client proxy with the correct identifier. 
        /// This identifier should be unique.
        /// </summary>
        /// <param name="identifier">Uniquely identifies the client proxy.</param>
        /// <param name="binding">The binding used to connect with the service.
        /// </param>
        /// <param name="endpointAddress">The address of the service.</param>
        /// <returns>The client proxy.</returns>
        public TInterface GetInstance<TInterface>(String identifier,
            String endpointConfigurationName) where TInterface : class
        {
            TInterface interfaceInstance = null;

            Object generatedInstance = null;

            if (_cachedClientProxies.TryGetValue(identifier, out generatedInstance))
            {
                interfaceInstance = (TInterface)generatedInstance;
            }
            else
            {
                Type iType = typeof(TInterface);
                string typeIdentifier = iType.Name;

                // Remove the 'I' from the type name. 
                if (iType.Name.StartsWith("I") && char.IsUpper(iType.Name, 1))
                {
                    typeIdentifier = iType.Name.Substring(1);
                }

                _classBuilder.SetClientEndpointSetting(ClientEndpointSetting.ConfigurationName);

                // The interface is not yet present in the cache, so let's 
                // create one and also add it to the cache.
                Type type = _classBuilder.GenerateType<TInterface>(typeIdentifier);

                // Create an instance of the proxy and construct it with the 
                // passed binding and endpoint address.
                var _generatedInstance = Activator.CreateInstance(type,
                    new object[] { endpointConfigurationName });

                interfaceInstance = (TInterface)_generatedInstance;

                ((ClientProxy<TInterface>)_generatedInstance)._identifier = identifier;

                _cachedClientProxies.Add(identifier, _generatedInstance);
            }

            return interfaceInstance;
        }

        /// <summary>
        /// Get an instance of the client proxy with the correct identifier. 
        /// This identifier should be unique.
        /// </summary>
        /// <param name="identifier">Uniquely identifies the client proxy.</param>
        /// <param name="binding">The binding used to connect with the service.
        /// </param>
        /// <param name="endpointAddress">The address of the service.</param>
        /// <returns>The client proxy.</returns>
        public ClientProxy<TInterface> GetInstance<TInterface>(String identifier,
            String endpointConfigurationName, Boolean returnClient = true) where TInterface : class
        {
            ClientProxy<TInterface> clientInstance = null;

            Object generatedInstance = null;

            if (_cachedClientProxies.TryGetValue(identifier, out generatedInstance))
            {
                clientInstance = (ClientProxy<TInterface>)generatedInstance;
            }
            else
            {
                Type iType = typeof(TInterface);
                string typeIdentifier = iType.Name;

                // Remove the 'I' from the type name. 
                if (iType.Name.StartsWith("I") && char.IsUpper(iType.Name, 1))
                {
                    typeIdentifier = iType.Name.Substring(1);
                }

                _classBuilder.SetClientEndpointSetting(ClientEndpointSetting.ConfigurationName);

                // The interface is not yet present in the cache, so let's 
                // create one and also add it to the cache.
                Type type = _classBuilder.GenerateType<TInterface>(typeIdentifier);

                // Create an instance of the proxy and construct it with the 
                // passed binding and endpoint address.
                var _generatedInstance = Activator.CreateInstance(type,
                    new object[] { endpointConfigurationName });

                clientInstance = (ClientProxy<TInterface>)_generatedInstance;

                ((ClientProxy<TInterface>)_generatedInstance)._identifier = identifier;

                _cachedClientProxies.Add(identifier, _generatedInstance);
            }

            return clientInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisposeAllCachedProxies()
        {
            foreach (var proxy in _cachedClientProxies)
            {
                this.DisposeCachedProxy(proxy.Key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisposeCachedProxy(string identifier)
        {
            var _proxy = _cachedClientProxies[identifier];

            ((ClientProxy)_proxy).isInternalCall = true;

            ((ClientProxy)_proxy).CloseConnectionForClientProxy();

            _cachedClientProxies.Remove(identifier);
        }

        #endregion

        #region Helper Methods



        #endregion

        #region Fields

        /// <summary>
        /// This will contain the client proxies which are already created.
        /// </summary>
        private Dictionary<string, Object> _cachedClientProxies;

        /// <summary>
        /// 
        /// </summary>
        private ProxyClassBuilder _classBuilder;

        #endregion
    }
}
