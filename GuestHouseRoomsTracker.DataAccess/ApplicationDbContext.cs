using GuestHouseRoomsTracker.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestHouseRoomsTracker.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private bool seedDb;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, bool seedDb = true) : base(options)
        //{
        //    if (this.Database.IsRelational())
        //    {
        //        this.Database.Migrate();
        //    }
        //    else
        //    {
        //        this.Database.EnsureCreated();
        //    }
        //    this.seedDb = seedDb;
        //}
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }    
        public override DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(rm => rm.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Reservations)
                .WithOne(res => res.Room)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
