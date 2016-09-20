using System.IdentityModel.Tokens;
using System.ServiceModel.Security;
using System.Xml;

namespace Mammatus.ServiceModel.Security.Authentication
{
    public class MsTokenSerializer : WSSecurityTokenSerializer
    {
        private MsTokenSerializerSettings _settings = new MsTokenSerializerSettings();

        public MsTokenSerializer(MsTokenSerializerSettings settings)
            : base(settings.SecurityVersion)
        {
            _settings = settings;
        }

        protected override void WriteTokenCore(XmlWriter writer, SecurityToken securityToken)
        {
            UserNameSecurityToken token = (UserNameSecurityToken)securityToken;

            writer.WriteRaw(string.Format(
            "<{0}:UsernameToken {1}:Id=\"" + securityToken.Id +
            "\" xmlns:{1}=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
            " xmlns:{0}=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
            "<{0}:Username>" + token.UserName + "</{0}:Username>" +
            "<{0}:Password {0}:Type=\"" + _settings.PasswordType + "\">" +
            _settings.GetPassword(token.Password, true) + "</{0}:Password>" +
            "<{0}:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">" +
            _settings.Nonce + "</{0}:Nonce>" +
            "<{1}:Created>" + _settings.CreatedDate + "</{1}:Created></{0}:UsernameToken>", _settings.TokenNamespace, _settings.TokenElementNamespace));
        }
    }
}
