using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entites;
using ProductService.Infrastructure.Data.EntityConfigurations;

namespace ProductService.Infrastructure.Data;

public class ProductServiceContext : DbContext
{
    public ProductServiceContext(DbContextOptions<ProductServiceContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductItemEntityTypeConfiguration());
    }
}
