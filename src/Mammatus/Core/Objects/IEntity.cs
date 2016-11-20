using System;

namespace Mammatus.Core.Object
{
    public interface IEntity
    {
        object EntityId { get; }
    }

    public interface IEntity<TKey> : IEntity where TKey : struct
    {
        TKey Id { get; }
        DateTime CreateDate { get; set; }
        TKey CreateUser { get; set; }
        DateTime? EditDate { get; set; }
        TKey? EditUser { get; set; }
        DateTime? DeleteDate { get; set; }
        TKey? DeleteUser { get; set; }
    }
}
