using Mammatus.Core.State;
using System;
using System.ComponentModel;

namespace Mammatus.Domain.UnitOfWork
{
    public abstract class BaseUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Current
        {
            get
            {
                return ContextState.Get<IUnitOfWork>("yada");
            }
            private set
            {
                ContextState.Store("yada", value);
            }
        }

        public IUnitOfWork Create()
        {
            IUnitOfWork previousUnitOfWork = this.Current;

            if (previousUnitOfWork != null)
            {
                return previousUnitOfWork;
            }

            INestableUnitOfWork newUnitOfWork = InternalCreate(previousUnitOfWork);
            this.Current = newUnitOfWork;
            return newUnitOfWork;
        }

        public IUnitOfWork Create(UnitOfWorkOption unitOfWorkOption)
        {
            if (!Enum.IsDefined(typeof(UnitOfWorkOption), unitOfWorkOption))
                throw new InvalidEnumArgumentException(nameof(unitOfWorkOption), (int)unitOfWorkOption,
                    typeof(UnitOfWorkOption));
            IUnitOfWork previousUnitOfWork = this.Current;

            if (unitOfWorkOption == UnitOfWorkOption.Reuse && previousUnitOfWork != null)
            {
                return previousUnitOfWork;
            }

            INestableUnitOfWork newUnitOfWork = InternalCreate(previousUnitOfWork);
            this.Current = newUnitOfWork;
            return newUnitOfWork;
        }

        public void UpdateCurrent(INestableUnitOfWork unitOfWork)
        {
            this.Current = unitOfWork.Previous;
        }

        protected abstract INestableUnitOfWork InternalCreate(IUnitOfWork previousUnitOfWork);
    }
}
