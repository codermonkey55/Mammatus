using System.Security.Principal;

namespace Mammatus.Security
{
    public interface ICoreIdentity : IIdentity
    {
        string Id
        {
            get;
        }
    }
}