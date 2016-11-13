namespace Mammatus.Domain.UnitOfWork
{
    public interface INestableUnitOfWork : IUnitOfWork
    {
        IUnitOfWork Previous { get; }
    }
}