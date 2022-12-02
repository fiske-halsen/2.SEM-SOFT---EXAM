﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserService.Context;

#nullable disable

namespace UserService.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class DbApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("UserService.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CityInfoId")
                        .HasColumnType("int");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityInfoId");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityInfoId = 1,
                            StreetName = "Skovledet"
                        },
                        new
                        {
                            Id = 2,
                            CityInfoId = 5,
                            StreetName = "Cphbusinessvej"
                        });
                });

            modelBuilder.Entity("UserService.Models.CityInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "Hillerød",
                            ZipCode = "3400"
                        },
                        new
                        {
                            Id = 2,
                            City = "Fredensborg",
                            ZipCode = "3480"
                        },
                        new
                        {
                            Id = 3,
                            City = "Taastrup",
                            ZipCode = "2630"
                        },
                        new
                        {
                            Id = 4,
                            City = "Hedehusene",
                            ZipCode = "2640"
                        },
                        new
                        {
                            Id = 5,
                            City = "Charlottenlund",
                            ZipCode = "2920"
                        },
                        new
                        {
                            Id = 6,
                            City = "CityTest",
                            ZipCode = "3000"
                        });
                });

            modelBuilder.Entity("UserService.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("RoleType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleType = "Customer"
                        },
                        new
                        {
                            Id = 2,
                            RoleType = "DeliveryPerson"
                        },
                        new
                        {
                            Id = 3,
                            RoleType = "RestaurantOwner"
                        });
                });

            modelBuilder.Entity("UserService.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AddressId = 1,
                            CreatedAt = new DateTime(2022, 12, 1, 10, 18, 34, 556, DateTimeKind.Utc).AddTicks(9876),
                            Email = "phillip.andersen1999@gmail.com",
                            FirstName = "Phillip",
                            ModifiedAt = new DateTime(2022, 12, 1, 10, 18, 34, 556, DateTimeKind.Utc).AddTicks(9878),
                            Password = "$2a$11$GKITrmfmvtCJ/Ta8nNyGe.gjgk16xMHY068siraW.HGZaOTrGWdd.",
                            RoleId = 1
                        },
                        new
                        {
                            Id = 2,
                            AddressId = 2,
                            CreatedAt = new DateTime(2022, 12, 1, 10, 18, 34, 670, DateTimeKind.Utc).AddTicks(88),
                            Email = "lukasbangstoltz@gmail.com",
                            FirstName = "Lukas",
                            ModifiedAt = new DateTime(2022, 12, 1, 10, 18, 34, 670, DateTimeKind.Utc).AddTicks(94),
                            Password = "$2a$11$ZI.9RkjX5VshfvrJpsyJduqzAAwTB8uhxoRAtcliO.gaZAZ5mZX8W",
                            RoleId = 3
                        },
                        new
                        {
                            Id = 3,
                            AddressId = 2,
                            CreatedAt = new DateTime(2022, 12, 1, 10, 18, 34, 783, DateTimeKind.Utc).AddTicks(5258),
                            Email = "christofferiw@gmail.com",
                            FirstName = "Christoffer",
                            ModifiedAt = new DateTime(2022, 12, 1, 10, 18, 34, 783, DateTimeKind.Utc).AddTicks(5264),
                            Password = "$2a$11$firrf1Ua4JJ5xE5qSSnXm.rcKrpCT/06EGtJxEvZI0sthe/2ISwjy",
                            RoleId = 2
                        });
                });

            modelBuilder.Entity("UserService.Models.Address", b =>
                {
                    b.HasOne("UserService.Models.CityInfo", "CityInfo")
                        .WithMany("Addresses")
                        .HasForeignKey("CityInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CityInfo");
                });

            modelBuilder.Entity("UserService.Models.User", b =>
                {
                    b.HasOne("UserService.Models.Address", "Address")
                        .WithMany("Users")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UserService.Models.Address", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("UserService.Models.CityInfo", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("UserService.Models.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
