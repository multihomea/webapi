using Microsoft.EntityFrameworkCore;

namespace ProductManager
{
    public class ProductManagerContext : DbContext
    {
        public ProductManagerContext(DbContextOptions<ProductManagerContext> options):base(options)
        { 
        }
        public DbSet<Product> Products { get; set; }

    }

}