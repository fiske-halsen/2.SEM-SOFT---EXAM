﻿// <auto-generated />
using System;
using DeliveryService.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeliveryService.Migrations
{
    [DbContext(typeof(DbApplicationContext))]
    [Migration("20221209152347_DeliveryService_first")]
    partial class DeliveryService_first
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DeliveryService.Models.Delivery", b =>
                {
                    b.Property<int>("DeliveryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeliveryId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryPersonId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDelivered")
                        .HasColumnType("bit");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeToDelivery")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isCancelled")
                        .HasColumnType("bit");

                    b.HasKey("DeliveryId");

                    b.ToTable("Deliveries");

                    b.HasData(
                        new
                        {
                            DeliveryId = 1,
                            CreatedDate = new DateTime(2022, 12, 9, 15, 23, 47, 713, DateTimeKind.Utc).AddTicks(2082),
                            DeliveryPersonId = 3,
                            IsDelivered = false,
                            OrderId = 1,
                            RestaurantId = 1,
                            TimeToDelivery = new DateTime(2022, 12, 9, 15, 53, 47, 713, DateTimeKind.Utc).AddTicks(2083),
                            UserEmail = "phillip.andersen1999@gmail.com",
                            isCancelled = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}