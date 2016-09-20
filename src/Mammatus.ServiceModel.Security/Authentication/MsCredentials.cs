using System.IdentityModel.Selectors;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace Mammatus.ServiceModel.Security.Authentication
{
    public class MsCredentials : ClientCredentials
    {
        public string TokenNamespace { get; set; }
        public string TokenElementNamespace { get; set; }
        public string CreatedDate { get; set; }

        private readonly SecurityVersion _securityVersion;

        public MsCredentials(SecurityVersion securityVersion)
        {
            _securityVersion = securityVersion;
        }

        protected MsCredentials(MsCredentials cc)
            : base(cc)
        {

        }

        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            MsTokenSerializerSettings settings = new MsTokenSerializerSettings
            {
                SecurityVersion = _securityVersion
            };

            if (!string.IsNullOrEmpty(TokenNamespace)) settings.TokenNamespace = TokenNamespace;
            if (!string.IsNullOrEmpty(TokenElementNamespace)) settings.TokenElementNamespace = TokenElementNamespace;
            if (!string.IsNullOrEmpty(CreatedDate)) settings.CreatedDate = CreatedDate;

            return new MsSecurityTokenManager(this);
        }

        protected override ClientCredentials CloneCore()
        {
            return new MsCredentials(this);
        }
    }
}
