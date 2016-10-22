using System;

namespace Mammatus.Data.Contracts
{
    public interface IUnitOfWork<TRepo> where TRepo : ISessionRepository
    {
        void Enroll<TDerivedRepo>(TDerivedRepo repository) where TDerivedRepo : TRepo;

        TRepo this[Type repoType] { get; }
    }
}
