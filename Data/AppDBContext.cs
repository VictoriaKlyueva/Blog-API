using BackendLaboratory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlackToken> BlackTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
