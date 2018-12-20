using Microsoft.EntityFrameworkCore;

namespace DDNS.Entity
{
    public class DDNSDbContext : DbContext
    {
        public DDNSDbContext(DbContextOptions<DDNSDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}