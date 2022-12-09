﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestaurantService.Context;

#nullable disable

namespace RestaurantService.Migrations
{
    [DbContext(typeof(DBApplicationContext))]
    [Migration("20221208195901_errortest2")]
    partial class errortest2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Common.ErrorModels.ExceptionDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ExceptionDtos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Message = "error msg",
                            StatusCode = 400
                        });
                });

            modelBuilder.Entity("RestaurantService.Model.Address", b =>
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
                            StreetName = "Skovvej"
                        },
                        new
                        {
                            Id = 2,
                            CityInfoId = 1,
                            StreetName = "Hovmarksvej"
                        });
                });

            modelBuilder.Entity("RestaurantService.Model.CityInfo", b =>
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

                    b.ToTable("CityInfos");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "Gentofte",
                            ZipCode = "2920"
                        });
                });

            modelBuilder.Entity("RestaurantService.Model.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("Menus");

                    b.HasData(
                        new
                        {
                            Id = 1
                        },
                        new
                        {
                            Id = 2
                        });
                });

            modelBuilder.Entity("RestaurantService.Model.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("StockCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "salatpizza",
                            Price = 79.989999999999995,
                            StockCount = 10
                        },
                        new
                        {
                            Id = 2,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "Peperoni",
                            Price = 79.230000000000004,
                            StockCount = 100
                        },
                        new
                        {
                            Id = 3,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "Calzone",
                            Price = 89.989999999999995,
                            StockCount = 0
                        },
                        new
                        {
                            Id = 4,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "ChokoladeIs",
                            Price = 39.990000000000002,
                            StockCount = 0
                        },
                        new
                        {
                            Id = 5,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "vaniljeis",
                            Price = 39.990000000000002,
                            StockCount = 0
                        },
                        new
                        {
                            Id = 6,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "chokoladekage",
                            Price = 39.990000000000002,
                            StockCount = 0
                        },
                        new
                        {
                            Id = 7,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "Cola",
                            Price = 19.989999999999998,
                            StockCount = 0
                        },
                        new
                        {
                            Id = 8,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "Fanta",
                            Price = 19.989999999999998,
                            StockCount = 0
                        },
                        new
                        {
                            Id = 9,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "Mayo",
                            Price = 9.9900000000000002,
                            StockCount = 0
                        },
                        new
                        {
                            Id = 10,
                            Description = "wow smager godt",
                            MenuId = 1,
                            Name = "Ketchup",
                            Price = 9.9900000000000002,
                            StockCount = 0
                        });
                });

            modelBuilder.Entity("RestaurantService.Model.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("MenuId")
                        .IsUnique();

                    b.ToTable("Restaurants");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AddressId = 1,
                            MenuId = 1,
                            Name = "PizzaPusheren",
                            OwnerId = 1
                        },
                        new
                        {
                            Id = 2,
                            AddressId = 2,
                            MenuId = 2,
                            Name = "SushiSlyngeren",
                            OwnerId = 2
                        });
                });

            modelBuilder.Entity("RestaurantService.Model.Address", b =>
                {
                    b.HasOne("RestaurantService.Model.CityInfo", "CityInfo")
                        .WithMany("Addresses")
                        .HasForeignKey("CityInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CityInfo");
                });

            modelBuilder.Entity("RestaurantService.Model.MenuItem", b =>
                {
                    b.HasOne("RestaurantService.Model.Menu", "Menu")
                        .WithMany("MenuItems")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("RestaurantService.Model.Restaurant", b =>
                {
                    b.HasOne("RestaurantService.Model.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestaurantService.Model.Menu", "Menu")
                        .WithOne("Restaurant")
                        .HasForeignKey("RestaurantService.Model.Restaurant", "MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("RestaurantService.Model.CityInfo", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("RestaurantService.Model.Menu", b =>
                {
                    b.Navigation("MenuItems");

                    b.Navigation("Restaurant")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
