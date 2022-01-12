using Microsoft.EntityFrameworkCore;
using wygrzebapi.Models;

namespace wygrzebapi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
          
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Search> Searches { get; set; }

        public DbSet<Article> Articles { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseSerialColumns();

            builder.Entity<User>(e => {
                e.Property(u => u.Login).HasColumnType("varchar(20)");
                e.Property(u => u.Password).HasColumnType("varchar(32)");
                e.Property(u => u.CreationDate).HasColumnType("timestamp");
            });

            builder.Entity<Article>(e => {
                e.Property(a => a.Title).HasColumnType("varchar(60)");
                e.Property(a => a.Content).HasColumnType("varchar(1000)");
                e.Property(a => a.CreationDate).HasColumnType("timestamp");
            });

            builder.Entity<Search>(e => {
                e.Property(s => s.Query).HasColumnType("varchar(140)");
                e.Property(s => s.TimeStamp).HasColumnType("timestamp");
            });
        }
    }
}
