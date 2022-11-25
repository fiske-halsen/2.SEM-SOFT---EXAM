using FeedbackService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FeedbackService.Context
{

        public class DbOrderServiceContext : DbContext
        {
            public DbOrderServiceContext(DbContextOptions<DbOrderServiceContext> options) : base(options) { }

            public DbSet<Review> Orders { get; set; }
            public DbSet<Complaint> Items { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }
        }
    }

