using BackendLaboratory.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<LocalUser> Users { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=usersdb;Username=postgres;Password=");
        }
    }
}
