using Microsoft.EntityFrameworkCore;
using CampLib.Model; // User
using KlasseLib;     // Room, Issue, Category

namespace KlasseLib
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Issue> Issues { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!; // <-- 🔥 tilføjet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CATEGORY: self-reference
            modelBuilder.Entity<Category>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ISSUE → REPORTER relation
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Reporter)
                .WithMany()
                .HasForeignKey(i => i.ReporterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ISSUE → ASSIGNED TO relation
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.AssignedTo)
                .WithMany()
                .HasForeignKey(i => i.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ISSUE → ROOM relation
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Room)
                .WithMany()
                .HasForeignKey(i => i.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // ISSUE → CATEGORY relation
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Category)
                .WithMany()
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}