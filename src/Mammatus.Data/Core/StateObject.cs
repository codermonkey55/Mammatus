using Mammatus.Data.Enums;

namespace Mammatus.Data.Core
{
    public class StateObject : IStateObject
    {
        public ObjectState State { get; set; }
    }
}
