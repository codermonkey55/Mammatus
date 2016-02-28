using System.Web;

namespace Mammatus.Core.State
{
    public interface IContextProvider
    {
        bool IsWebApplication { get; }

        bool IsWcfApplication { get; }

        bool IsAspNetCompatEnabled { get; }
    }
}