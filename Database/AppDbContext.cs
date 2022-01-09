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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseSerialColumns();
        }
    }
}
