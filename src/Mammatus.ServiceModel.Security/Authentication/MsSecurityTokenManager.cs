using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace Mammatus.ServiceModel.Security.Authentication
{
    public class MsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        public MsTokenSerializerSettings CustomTokenSerializerSettings { get; set; }

        public MsSecurityTokenManager(MsCredentials cred)
            : base(cred)
        {
            CustomTokenSerializerSettings = new MsTokenSerializerSettings();
        }

        public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
        {
            return new MsTokenSerializer(CustomTokenSerializerSettings);
        }
    }
}
