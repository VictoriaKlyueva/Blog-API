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
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityUser> CommunityUsers { get; set; }
        public DbSet<BlackToken> BlackTokens { get; set; }
        public DbSet<TagDto> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Community>()
                .HasMany(c => c.Users)
                .WithMany(s => s.Communities)
                .UsingEntity<CommunityUser>(
                   j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.CommunityUsers)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Community)
                    .WithMany(p => p.CommunityUsers)
                    .HasForeignKey(pt => pt.CommunityId),
                j =>
                {
                    j.HasKey(t => new { t.CommunityId, t.UserId });
                    j.ToTable("CommunityUsers");
                });
        }
    }
}
