using DDNS.Entity.Users;
using Microsoft.EntityFrameworkCore;

namespace DDNS.Entity
{
    public class DDNSDbContext : DbContext
    {
        public DDNSDbContext(DbContextOptions<DDNSDbContext> options) : base(options) { }

        public DbSet<UsersEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}