using Mammatus.Core.IoC;

namespace Mammatus.Domain.UnitOfWork
{
    public abstract class AbstractUnitOfWork : IUnitOfWork
    {
        public static IUnitOfWork Start(UnitOfWorkOption unitOfWorkOption = UnitOfWorkOption.Reuse)
        {
            //IUnitOfWorkFactory factory = IContainerAdapter.GetInstance<IUnitOfWorkFactory>();

            IUnitOfWorkFactory factory = UnitOfWorkFactory.Instance;

            return factory.Create(unitOfWorkOption);
        }

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public void RollbackChanges()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}