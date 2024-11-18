using BackendLaboratory.Data.DTO;
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
        public DbSet<TagDto> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
