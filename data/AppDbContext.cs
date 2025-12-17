using Microsoft.EntityFrameworkCore;
using UrlHealthMonitor.Models;

namespace UrlHealthMonitor.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<MonitoredUrl> MonitoredUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for UserGroup
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.AppUserId, ug.GroupId });
        }
    }
}


