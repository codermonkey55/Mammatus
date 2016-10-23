using Microsoft.EntityFrameworkCore;

namespace Mammatus.Data.Entity.Core
{
    class Program
    {
        public void Test()
        {
            // Create options telling the context to use an
            // InMemory database and the service provider.
            var builder = new DbContextOptionsBuilder<EFDbContext>();
            builder.UseInMemoryDatabase("PocoLib.Database");

            var _contextOptions = builder.Options;

            var uow = new EFUnitOfWork(new EFDbContext(_contextOptions));

            var repo = new FakeEFGenericRepository(null);

            uow.Enroll(repo);

            var fe = new FakeEntity();

            repo.Add(fe);

            fe.Identifier = "FakeEnity";

            repo.Edit(fe);

            uow.Commit();
        }
    }

    public class FakeEntity
    {
        public string Identifier { get; set; }
    }

    public class FakeEFGenericRepository : EFGenericRepository<FakeEntity>
    {
        public FakeEFGenericRepository(IEFEntityCollection<FakeEntity> entityCollection) : base(entityCollection)
        {

        }
    }
}
