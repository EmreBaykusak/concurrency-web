using Microsoft.EntityFrameworkCore;

namespace Concurreny.Web.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .UseXminAsConcurrencyToken();


            modelBuilder
                .Entity<Product>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            
            base.OnModelCreating(modelBuilder);
        }
    }
}
