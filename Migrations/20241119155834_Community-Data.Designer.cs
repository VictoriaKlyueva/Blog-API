﻿// <auto-generated />
using System;
using BackendLaboratory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendLaboratory.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20241119155834_Community-Data")]
    partial class CommunityData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BackendLaboratory.Data.Entities.BlackToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Blacktoken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BlackTokens");
                });

            modelBuilder.Entity("BackendLaboratory.Data.Entities.Community", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SubscribersCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Communities");

                    b.HasData(
                        new
                        {
                            Id = new Guid("21db62c6-a964-45c1-17e0-08dbea521a96"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6622),
                            Description = "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами",
                            IsClosed = true,
                            Name = "Масонская ложа",
                            SubscribersCount = 44
                        },
                        new
                        {
                            Id = new Guid("c5639aab-3a25-4efc-17e1-08dbea521a96"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6695),
                            Description = "Без длинных предисловий: мужчина умер",
                            IsClosed = false,
                            Name = "Следствие вели с Л. Каневским",
                            SubscribersCount = 34
                        },
                        new
                        {
                            Id = new Guid("b9851a35-b836-47f8-17e2-08dbea521a96"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6705),
                            Description = "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления",
                            IsClosed = false,
                            Name = "IT <3",
                            SubscribersCount = 31
                        });
                });

            modelBuilder.Entity("BackendLaboratory.Data.Entities.CommunityUser", b =>
                {
                    b.Property<Guid>("CommunityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("CommunityId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("CommunityUsers", (string)null);
                });

            modelBuilder.Entity("BackendLaboratory.Data.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7dd0e51a-606f-4dea-e6e3-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6519),
                            Name = "история"
                        },
                        new
                        {
                            Id = new Guid("d774dd11-2600-46ab-e6e4-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6539),
                            Name = "еда"
                        },
                        new
                        {
                            Id = new Guid("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6543),
                            Name = "18+"
                        },
                        new
                        {
                            Id = new Guid("553f5361-428a-4122-e6e6-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6553),
                            Name = "приколы"
                        },
                        new
                        {
                            Id = new Guid("86acb301-05ff-4822-e6e7-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6563),
                            Name = "it"
                        },
                        new
                        {
                            Id = new Guid("e587312f-4df7-4879-e6e8-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6573),
                            Name = "интернет"
                        },
                        new
                        {
                            Id = new Guid("47aa0a33-2b86-4039-e6e9-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6583),
                            Name = "теория_заговора"
                        },
                        new
                        {
                            Id = new Guid("e463050a-d659-433d-e6ea-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6592),
                            Name = "соцсети"
                        },
                        new
                        {
                            Id = new Guid("0c64569f-5675-484b-e6eb-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6600),
                            Name = "косплей"
                        },
                        new
                        {
                            Id = new Guid("fb1f2ce1-6943-420f-e6ec-08dbea521a91"),
                            CreateTime = new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6612),
                            Name = "преступление"
                        });
                });

            modelBuilder.Entity("BackendLaboratory.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackendLaboratory.Data.Entities.CommunityUser", b =>
                {
                    b.HasOne("BackendLaboratory.Data.Entities.Community", "Community")
                        .WithMany("CommunityUsers")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendLaboratory.Data.Entities.User", "User")
                        .WithMany("CommunityUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackendLaboratory.Data.Entities.Community", b =>
                {
                    b.Navigation("CommunityUsers");
                });

            modelBuilder.Entity("BackendLaboratory.Data.Entities.User", b =>
                {
                    b.Navigation("CommunityUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
