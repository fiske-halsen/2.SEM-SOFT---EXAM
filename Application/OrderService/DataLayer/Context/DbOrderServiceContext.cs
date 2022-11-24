using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.DataLayer.Context
{
    public class DbOrderServiceContext : DbContext
    {
        public DbOrderServiceContext(DbContextOptions<DbOrderServiceContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
