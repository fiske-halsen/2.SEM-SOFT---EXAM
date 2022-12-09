using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Context
{
    public class DbApplicationContext : DbContext
    {
        public DbApplicationContext(DbContextOptions<DbApplicationContext> options) : base(options)
        {
        }

        public DbSet<Delivery> Deliveries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Seed(builder);
            base.OnModelCreating(builder);
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>()
                .HasData(
                    new Delivery
                    {
                        DeliveryId = 1,
                        DeliveryPersonId = 3,
                        RestaurantId = 1,
                        OrderId = 1,
                        UserEmail = "phillip.andersen1999@gmail.com",
                        CreatedDate = DateTime.UtcNow,
                        IsDelivered = false,
                        TimeToDelivery = DateTime.UtcNow.AddMinutes(30),
                    }
                );
        }
    }
}