using Mammatus.Domain.Enums;
using System;
using System.ComponentModel;

namespace Mammatus.Domain.UnitOfWork
{
    public abstract class AbstractUnitOfWork : IUnitOfWork
    {
        public static IUnitOfWork Start()
        {
            //IUnitOfWorkFactory factory = IContainerAdapter.GetInstance<IUnitOfWorkFactory>();

            IUnitOfWorkFactory factory = UnitOfWorkFactory.Instance;

            return factory.Create(UnitOfWorkOption.Reuse);
        }

        public static IUnitOfWork Start(UnitOfWorkOption unitOfWorkOption)
        {
            if (!Enum.IsDefined(typeof(UnitOfWorkOption), unitOfWorkOption))
                throw new InvalidEnumArgumentException(nameof(unitOfWorkOption), (int)unitOfWorkOption,
                    typeof(UnitOfWorkOption));

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