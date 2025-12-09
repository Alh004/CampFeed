using Microsoft.EntityFrameworkCore;

namespace KlasseLib
{
    public class AppDbContext : DbContext
    {
        // ctor som Webserveren kan bruge til at give options (connection string osv.)
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
 
        // Tabeller (DbSet) – start bare med de tre vigtigste
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Issue> Issues { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        // Her kan du senere konfigurere relationer, hvis der er behov
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Eksempel: Category kan have ParentCategory (self reference)
            modelBuilder.Entity<Category>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}   