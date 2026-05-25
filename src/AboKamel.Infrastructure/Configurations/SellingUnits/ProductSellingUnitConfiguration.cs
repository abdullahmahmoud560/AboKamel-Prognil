using AboKamel.Domain.Entities.SellingUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AboKamel.Infrastructure.Configurations.SellingUnits;

public class ProductSellingUnitConfiguration : IEntityTypeConfiguration<ProductSellingUnit>
{
    public void Configure(EntityTypeBuilder<ProductSellingUnit> builder)
    {
        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.Property(x => x.Quantity)
            .IsRequired();
    }
}
