using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Context
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Seed(builder);
            base.OnModelCreating(builder);
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            // Dummy data
            modelBuilder.Entity<Order>()
                .HasData(new Order
                {
                    Id = 1, CustomerEmail = "phillip.andersen1999@gmail.com", IsActive = true, IsApproved = false,
                    TotalPrice = 367.0, RestaurantId = 1
                });


            modelBuilder.Entity<OrderItem>()
                .HasData(new OrderItem {Id = 1, OrderId = 1, ItemPrice = 367.0});
        }
    }
}