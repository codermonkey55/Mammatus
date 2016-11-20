namespace Mammatus.Core.Object
{
    public interface ICrudEntity<T> where T : class
    {
        T NewEntity();
        int Add();
        bool Update();
        bool Remove();
    }
}
