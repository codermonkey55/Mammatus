using System.Data.Entity;

namespace Mammatus.Data.EF
{
    class Program
    {
        public void Test()
        {
            var uow = new EFUnitOfWork(new DbContext(string.Empty));

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
