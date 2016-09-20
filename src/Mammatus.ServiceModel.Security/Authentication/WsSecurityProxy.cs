using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace Mammatus.ServiceModel.Security.Authentication
{
    public class WsSecurityProxy
    {
        public int MaxMessageSize { get; set; }

        public T CreateSoap11Proxy<T>(string endpoint, string username, string password)
        {
            if (MaxMessageSize < 65536) throw new InvalidOperationException("MaxMessageSize is less than the default of 65536K.  Set MaxMessageSize >= 65536K");
            if (string.IsNullOrEmpty(endpoint)) throw new ArgumentException("Endpoint is blank.  Please use a valid endpoint");

            CustomBinding binding = new CustomBinding();

            var securityElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            securityElement.IncludeTimestamp = false;
            securityElement.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
            securityElement.MessageSecurityVersion = MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;

            var encoding = new TextMessageEncodingBindingElement();
            encoding.MessageVersion = MessageVersion.Soap11;

            var transport = new HttpsTransportBindingElement();
            transport.MaxReceivedMessageSize = MaxMessageSize;

            binding.Elements.Add(securityElement);
            binding.Elements.Add(encoding);
            binding.Elements.Add(transport);

            ChannelFactory<T> channelFactory = new ChannelFactory<T>(binding, new EndpointAddress(endpoint));
            MsCredentials credentials = new MsCredentials(SecurityVersion.WSSecurity11);
            credentials.UserName.UserName = username;
            credentials.UserName.Password = password;

            channelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();
            channelFactory.Endpoint.Behaviors.Add(credentials);

            return channelFactory.CreateChannel();
        }
    }
}
