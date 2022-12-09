using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Test.Context
{
    public class DbTestContext : DbContext
    {
        public DbTestContext(DbContextOptions<DbTestContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
