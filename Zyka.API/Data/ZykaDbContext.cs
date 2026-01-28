using System;
using Zyka.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Zyka.API.Data
{
    public class ZykaDbContext : DbContext
    {
        public ZykaDbContext(DbContextOptions<ZykaDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        // Override OnModelCreating to configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//Call the base method

            // Configure unique constraints(Won't allow duplicate entries)
            modelBuilder.Entity<User>().HasIndex(u => u.EmailAddress).IsUnique();
        }
    }
}
