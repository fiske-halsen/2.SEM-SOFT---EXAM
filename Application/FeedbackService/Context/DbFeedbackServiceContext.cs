using FeedbackService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FeedbackService.Context
{

        public class DbOrderServiceContext : DbContext
        {
            public DbOrderServiceContext(DbContextOptions<DbOrderServiceContext> options) : base(options) { }

            public DbSet<Review> Reviews { get; set; }
            public DbSet<Complaint> Complaints { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }

        private void Seed(ModelBuilder builder)
        {
            builder.Entity<Review>().HasData(
              new Review { Id = 1, UserId = 1, RestaurantId = 2, DeliveryDriverId = 3, ReviewText = "Maden var god og blev leveret hurtigt", ReviewDate = DateTime.Now, Rating = 5 },
              new Review { Id = 2, UserId = 1, RestaurantId = 2, DeliveryDriverId = 3, ReviewText = "Maden var dårlig og blev leveret efter 3 timer", ReviewDate = DateTime.Now, Rating = 1 }

              );
            builder.Entity<Complaint>().HasData(
                new Complaint { Id = 1, UserId = 1, RestaurantId = 2, DeliveryDriverID = 3, ComplaintText = "Maden var kold, jeg vil gerne have mine penge tilbage!", ComplaintDate = DateTime.Today, OrderId = 1 },
                new Complaint { Id = 2, UserId = 1, RestaurantId = 2, DeliveryDriverID = 3, ComplaintText = "Maden var kold, jeg vil gerne have mine penge tilbage!", ComplaintDate = DateTime.Today, OrderId = 1 });

        }
    }
    }

