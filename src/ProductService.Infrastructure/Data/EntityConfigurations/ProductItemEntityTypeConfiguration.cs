using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entites;

namespace ProductService.Infrastructure.Data.EntityConfigurations;

public class ProductItemEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(p => p.Id)
            .UseHiLo("product_hilo")
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .IsRequired(true)
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .IsRequired(true);

        builder.Property(p => p.Active)
            .IsRequired(true);
    }
}
