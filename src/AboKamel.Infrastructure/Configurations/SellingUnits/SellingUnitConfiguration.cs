using AboKamel.Domain.Entities.SellingUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AboKamel.Infrastructure.Configurations.SellingUnits;

public class SellingUnitConfiguration : IEntityTypeConfiguration<SellingUnit>
{
    public void Configure(EntityTypeBuilder<SellingUnit> builder)
    {
        builder.Property(a => a.Name)
           .IsRequired()
           .HasMaxLength(100);
    }
}
