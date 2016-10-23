using Microsoft.EntityFrameworkCore;

namespace Mammatus.Data.Entity.Core
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
        {

        }

        public EFDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
