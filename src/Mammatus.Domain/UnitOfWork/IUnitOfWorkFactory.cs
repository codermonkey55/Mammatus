
namespace Mammatus.Domain.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(UnitOfWorkOption unitOfWorkOption = UnitOfWorkOption.Reuse);
    }
}
