using Microsoft.EntityFrameworkCore;

namespace eShop.API.Entities.Models
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Product> Products { get; set;}
        public DbSet<Category> Categories { get; set; }
    }
}
