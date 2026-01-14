using System;
using Zyka.Models;
using Microsoft.EntityFrameworkCore;

namespace Zyka.Data
{
    public class ZykaDbContext : DbContext
    {
        public ZykaDbContext(DbContextOptions<ZykaDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TableInfo> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }

        // Override OnModelCreating to configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//Call the base method

            // Configure unique constraints(Won't allow duplicate entries)
            modelBuilder.Entity<User>().HasIndex(u => u.EmailAddress).IsUnique();
            modelBuilder.Entity<TableInfo>().HasIndex(t => t.TableNumber).IsUnique();
            modelBuilder.Entity<Reservation>().HasIndex(r => new { r.TableId, r.ReservationDate, r.TimeSlotId }).IsUnique();
            modelBuilder.Entity<Payment>().HasOne(p => p.Reservation).WithOne(r => r.Payment).HasForeignKey<Payment>(p => p.ReservationId);
        }
    }
}