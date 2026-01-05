using Microsoft.EntityFrameworkCore;
using CampLib.Model;
using KlasseLib;
using KlasseLib.Model;

namespace KlasseLib
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {d
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ISSUE → ROOM
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Room)
                .WithMany()
                .HasForeignKey(i => i.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // ISSUE → CATEGORY
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Category)
                .WithMany()
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ISSUE → REPORTER USER
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Reporter)
                .WithMany()
                .HasForeignKey(i => i.ReporterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ISSUE → ASSIGNED TO USER
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.AssignedTo)
                .WithMany()
                .HasForeignKey(i => i.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ISSUE → DEPARTMENT
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.AssignedDepartment)
                .WithMany()
                .HasForeignKey(i => i.AssignedDepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
