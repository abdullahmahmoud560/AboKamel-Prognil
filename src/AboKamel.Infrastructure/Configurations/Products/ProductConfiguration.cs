using Capsula.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capsula.Infrastructure.Configurations.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.ImagePath)
            .HasMaxLength(500);

        builder.Property(x => x.MinimumQuantityPerOrder)
            .IsRequired();

        builder.Property(x => x.MaximumQuantityPerOrder)
            .IsRequired();
    }
}
