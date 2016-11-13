using Mammatus.Domain.Enums;

namespace Mammatus.Domain.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        IUnitOfWork Create(UnitOfWorkOption unitOfWorkOption);
    }
}
