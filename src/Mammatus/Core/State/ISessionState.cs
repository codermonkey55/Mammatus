

namespace Mammatus.Core.State
{
    public interface ISessionState : IState
    {
        string SessionId { get; }
    }
}