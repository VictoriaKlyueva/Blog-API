using BackendLaboratory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<LocalUser> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=localhost port=5432 dbname=shop user=postgres password=3348 connect_timeout=10 sslmode=prefer");
        }
    }
}
