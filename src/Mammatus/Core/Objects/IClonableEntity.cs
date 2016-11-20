namespace Mammatus.Core.Object
{
    public interface IClonableEntity<T> where T : class
    {
        T Clone();
    }
}
