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
        {
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Issue_Comment> IssueComments { get; set; }
        public DbSet<Issue_Image> Issue_Images { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }

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

            // ISSUE COMMENT
            modelBuilder.Entity<Issue_Comment>()
                .HasKey(c => c.Idcomment);

            modelBuilder.Entity<Issue_Comment>()
                .HasOne<Issue>()
                .WithMany()
                .HasForeignKey(c => c.IssueId);

            modelBuilder.Entity<Issue_Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.CreatedByUserId);

            // ISSUE IMAGE
            modelBuilder.Entity<Issue_Image>()
                .HasKey(i => i.Idimage);

            modelBuilder.Entity<Issue_Image>()
                .HasOne<Issue>()
                .WithMany()
                .HasForeignKey(i => i.IssueId);

            modelBuilder.Entity<Issue_Image>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(i => i.UploadedByUserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
