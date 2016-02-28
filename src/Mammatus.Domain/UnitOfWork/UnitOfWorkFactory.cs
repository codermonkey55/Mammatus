using Mammatus.Core.State;

namespace Mammatus.Domain.UnitOfWork
{
    public class UnitOfWorkFactory : AbstractUnitOfWorkFactory
    {
        public static UnitOfWorkFactory Instance
        {
            get
            {
                return new UnitOfWorkFactory();
            }
        }

        protected override IUnitOfWork InternalCreate(IUnitOfWork previousUnitOfWork)
        {
            throw new System.NotImplementedException();
        }
    }
}
