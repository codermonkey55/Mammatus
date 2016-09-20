using System;
using Mammatus.ServiceModel.Model;
using Mammatus.ServiceModel.Security.Authentication;

namespace Mammatus.ServiceModel.Security.Communicators
{
    public class WsSecurityAuthenticationCommunicator<T> : ICommunicator where T : IProxyInterface
    {
        #region ICommunicator Members

        public object InvokeOperation(object[] parameters)
        {
            WsSecurityProxy proxy = new WsSecurityProxy
            {
                MaxMessageSize = Int32.MaxValue
            };
            T myProxy = proxy.CreateSoap11Proxy<T>("http://www.exapmle.com", "user", "pass");
            return myProxy.InvokeProxyOperation(parameters);
        }

        #endregion
    }

    public interface IProxyInterface
    {
        string InvokeProxyOperation(object[] parameters);
    }
}