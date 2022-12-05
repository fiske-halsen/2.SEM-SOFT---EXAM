using FeedbackService.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackService.Context
{

    public class DBFeedbackServiceContext : DbContext
    {
        public DBFeedbackServiceContext(DbContextOptions<DBFeedbackServiceContext> options) : base(options) { }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            builder.Entity<Review>().HasData(
              new Review { Id = 1, ReviewText = "Maden var god og blev leveret hurtigt", ReviewDate = DateTime.Now, OrderId = 2, Rating = 5 },
              new Review { Id = 2, ReviewText = "Maden var dårlig og blev leveret efter 3 timer", ReviewDate = DateTime.Now, OrderId = 1, Rating = 1 }

              );
        }
    }
}

