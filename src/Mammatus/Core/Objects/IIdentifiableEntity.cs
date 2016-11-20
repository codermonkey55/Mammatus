namespace Mammatus.Core.Object
{
    interface IIdentifiableEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
