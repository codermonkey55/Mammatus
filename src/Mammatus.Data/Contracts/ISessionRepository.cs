namespace Mammatus.Data.Contracts
{
    public interface ISessionRepository : IRepository
    {
        void SetCollection(IEntityCollection entityCollection);
    }
}
