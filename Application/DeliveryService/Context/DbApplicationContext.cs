using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Emit;

namespace DeliveryService.Context
{
    public class DbApplicationContext : DbContext
    {
        public DbApplicationContext(DbContextOptions<DbApplicationContext> options) : base(options) { }

        public DbSet<Delivery> Delivery { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Seed(builder);
            base.OnModelCreating(builder);
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>()
                .HasData(
                    //DeliveryPerson = 3
                    //Customer = 1
                    new Delivery
                    {
                        DeliveryId = 1,
                        DeliveryPersonId = 3,
                        OrderId = 1,
                        UserEmail = "phillip.andersen1999@gmail.com",
                        Address = "Skovledet",
                        ZipCode = "3400"
                    }
                );

        }
    }
}
