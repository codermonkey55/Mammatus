namespace Mammatus.Data.Contracts
{
    public interface ICrudEntity<T> where T : class
    {
        T Create();

        int Add();

        bool Update();

        bool Delete();
    }
}
