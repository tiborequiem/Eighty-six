using Microsoft.EntityFrameworkCore;
using NetProject.Models;

namespace NetProject.Repository
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure email is unique across the app
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Index GoogleSubjectId for fast lookups
            modelBuilder.Entity<User>()
                .HasIndex(u => u.GoogleSubjectId);
        }
    }
}
