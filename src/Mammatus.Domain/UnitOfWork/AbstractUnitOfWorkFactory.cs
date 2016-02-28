using Mammatus.Core.State;

namespace Mammatus.Domain.UnitOfWork
{
    public abstract class AbstractUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IState currentContextState;

        public IUnitOfWork Current
        {
            get
            {
                return currentContextState.Get<IUnitOfWork>("yada");
            }
            private set
            {
                currentContextState.Store("yada", value);
            }
        }

        public AbstractUnitOfWorkFactory()
        {
            currentContextState = null;
        }

        public IUnitOfWork Create(UnitOfWorkOption unitOfWorkOption = UnitOfWorkOption.Reuse)
        {
            IUnitOfWork previousUnitOfWork = this.Current;

            if (unitOfWorkOption == UnitOfWorkOption.Reuse && previousUnitOfWork != null)
            {
                return previousUnitOfWork;
            }

            IUnitOfWork newUnitOfWork = InternalCreate(previousUnitOfWork);

            this.Current = newUnitOfWork;

            return newUnitOfWork;
        }

        public void UpdateCurrent(IUnitOfWork unitOfWork)
        {
            this.Current = unitOfWork;
        }

        protected virtual IUnitOfWork InternalCreate(IUnitOfWork previousUnitOfWork)
        {
            return new UnitOfWork();
        }
    }
}
