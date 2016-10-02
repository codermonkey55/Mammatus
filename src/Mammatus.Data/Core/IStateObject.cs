using Mammatus.Data.Enums;

namespace Mammatus.Data.Core
{
    public interface IStateObject
    {
        ObjectState State { get; set; }
    }
}
