using BackendLaboratory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendLaboratory.Data
{
    public partial class GarContext : DbContext
    {
        public GarContext()
        {
        }

        public GarContext(DbContextOptions<GarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AsAddrObj> AsAddrObjs { get; set; }

        public virtual DbSet<AsHouse> AsHouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsAddrObj>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("as_addr_obj");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Level).HasColumnName("level");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Objectguid).HasColumnName("objectguid");
                entity.Property(e => e.Typename).HasColumnName("typename");
                entity.Property(e => e.Parentobjid).HasColumnName("parentobjid");
                entity.Property(e => e.Path).HasColumnName("path");
            });

            modelBuilder.Entity<AsHouse>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("as_houses");

                entity.Property(e => e.Housenum).HasColumnName("housenum");
                entity.Property(e => e.Addnum1).HasColumnName("addnum1");
                entity.Property(e => e.Addnum2).HasColumnName("addnum2");
                entity.Property(e => e.Housetype).HasColumnName("housetype");
                entity.Property(e => e.Addtype1).HasColumnName("addtype1");
                entity.Property(e => e.Addtype2).HasColumnName("addtype2");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Objectguid).HasColumnName("objectguid");
                entity.Property(e => e.Parentobjid).HasColumnName("parentobjid");
                entity.Property(e => e.Path).HasColumnName("path");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
