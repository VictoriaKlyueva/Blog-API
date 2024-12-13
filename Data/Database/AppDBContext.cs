using BackendLaboratory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Data.Database
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<BlackToken> BlackTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommunityUser> CommunityUsers { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Like> LikesLink { get; set; }
        public DbSet<PostsUser> PostsUsers { get; set; }

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

            modelBuilder
                .Entity<Post>()
                .HasMany(c => c.Users)
                .WithMany(s => s.Posts)
                .UsingEntity<Like>(
                   j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.LikesLink)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Post)
                    .WithMany(p => p.LikesLink)
                    .HasForeignKey(pt => pt.PostId),
                j =>
                {
                    j.HasKey(t => new { t.PostId, t.UserId });
                    j.ToTable("Likes");
                });

            modelBuilder
                .Entity<Post>()
                .HasMany(c => c.Tags)
                .WithMany(s => s.Posts)
                .UsingEntity<PostTag>(
                   j => j
                    .HasOne(pt => pt.Tag)
                    .WithMany(t => t.PostTags)
                    .HasForeignKey(pt => pt.TagId),
                j => j
                    .HasOne(pt => pt.Post)
                    .WithMany(p => p.PostTags)
                    .HasForeignKey(pt => pt.PostId),
                j =>
                {
                    j.HasKey(t => new { t.PostId, t.TagId });
                    j.ToTable("PostTags");
                });

            modelBuilder
                .Entity<Post>()
                .HasMany(c => c.Users)
                .WithMany(s => s.Posts)
                .UsingEntity<PostsUser>(
                   j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.PostsUsers)
                    .HasForeignKey(pt => pt.UserId),
                    j => j
                        .HasOne(pt => pt.Post)
                        .WithMany(p => p.PostsUsers)
                        .HasForeignKey(pt => pt.PostId),
                    j =>
                    {
                        j.HasKey(t => new { t.PostId, t.UserId });
                        j.ToTable("PostsUsers");
                    });

            modelBuilder.Entity<Comment>()
               .HasMany(c => c.ChildComments)
               .WithOne(c => c.ParentComment)
               .HasForeignKey(c => c.ParentId)
               .OnDelete(DeleteBehavior.Cascade);

            AddAdminData(modelBuilder);
        }

        private void AddAdminData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Community>().HasData(
                new Community
                {
                    Name = "Масонская ложа",
                    Description = "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами",
                    IsClosed = true,
                    SubscribersCount = 0,
                    Id = Guid.Parse("21db62c6-a964-45c1-17e0-08dbea521a96"),
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106622"), DateTimeKind.Utc)
                },
                new Community
                {
                    Name = "Следствие вели с Л. Каневским",
                    Description = "Без длинных предисловий: мужчина умер",
                    IsClosed = false,
                    SubscribersCount = 0,
                    Id = Guid.Parse("c5639aab-3a25-4efc-17e1-08dbea521a96"),
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106695"), DateTimeKind.Utc)
                },
                new Community
                {
                    Name = "IT <3",
                    Description = "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления",
                    IsClosed = false,
                    SubscribersCount = 0,
                    Id = Guid.Parse("b9851a35-b836-47f8-17e2-08dbea521a96"),
                    CreateTime = DateTime.SpecifyKind(DateTime.Parse("2023-11-21T12:24:15.8106705"), DateTimeKind.Utc)
                }
            );
        }
    }
}
