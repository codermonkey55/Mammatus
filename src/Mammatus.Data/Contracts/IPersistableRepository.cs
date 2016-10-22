namespace Mammatus.Data.Contracts
{
    public interface IPersistableRepository : IRepository
    {
        int Persist();

        void Modify(object entity);
    }
}
