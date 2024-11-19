using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<Tag> Tags { get; set; }

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

            AddAdminData(modelBuilder);
        }

        private void AddAdminData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(
                new Tag { 
                    Name = "история", 
                    Id = Guid.Parse("7dd0e51a-606f-4dea-e6e3-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106519"), DateTimeKind.Utc) 
                },
                new Tag {
                    Name = "еда", 
                    Id = Guid.Parse("d774dd11-2600-46ab-e6e4-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106539"), DateTimeKind.Utc) 
                },
                new Tag { 
                    Name = "18+", 
                    Id = Guid.Parse("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106543"), DateTimeKind.Utc)
                },
                new Tag {
                    Name = "приколы", 
                    Id = Guid.Parse("553f5361-428a-4122-e6e6-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106553"), DateTimeKind.Utc) 
                },
                new Tag { 
                    Name = "it", 
                    Id = Guid.Parse("86acb301-05ff-4822-e6e7-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106563"), DateTimeKind.Utc) 
                },
                new Tag {
                    Name = "интернет", 
                    Id = Guid.Parse("e587312f-4df7-4879-e6e8-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106573"), DateTimeKind.Utc) 
                },
                new Tag {
                    Name = "теория_заговора", 
                    Id = Guid.Parse("47aa0a33-2b86-4039-e6e9-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106583"), DateTimeKind.Utc) 
                },
                new Tag { 
                    Name = "соцсети", 
                    Id = Guid.Parse("e463050a-d659-433d-e6ea-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106592"), DateTimeKind.Utc) 
                },
                new Tag {
                    Name = "косплей", 
                    Id = Guid.Parse("0c64569f-5675-484b-e6eb-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.810660"), DateTimeKind.Utc) 
                },
                new Tag { 
                    Name = "преступление", 
                    Id = Guid.Parse("fb1f2ce1-6943-420f-e6ec-08dbea521a91"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106612"), DateTimeKind.Utc) 
                }
            );

            modelBuilder.Entity<Community>().HasData(
                new Community { 
                    Name = "Масонская ложа", 
                    Description = "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами", 
                    IsClosed = true,
                    SubscribersCount = 44,
                    Id = Guid.Parse("21db62c6-a964-45c1-17e0-08dbea521a96"), 
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106622"), DateTimeKind.Utc) 
                },
                new Community
                {
                    Name = "Следствие вели с Л. Каневским",
                    Description = "Без длинных предисловий: мужчина умер",
                    IsClosed = false,
                    SubscribersCount = 34,
                    Id = Guid.Parse("c5639aab-3a25-4efc-17e1-08dbea521a96"),
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106695"), DateTimeKind.Utc)
                },
                new Community
                {
                    Name = "IT <3",
                    Description = "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления",
                    IsClosed = false,
                    SubscribersCount = 31,
                    Id = Guid.Parse("b9851a35-b836-47f8-17e2-08dbea521a96"),
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106705"), DateTimeKind.Utc)
                }
            );
        }
    }
}
