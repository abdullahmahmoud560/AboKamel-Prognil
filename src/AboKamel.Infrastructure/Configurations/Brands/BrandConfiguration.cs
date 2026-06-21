using Capsula.Domain.Entities.Brands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capsula.Infrastructure.Configurations.Brands;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Slug)
            .HasMaxLength(200);

        builder.Property(x => x.ImagePath)
            .HasMaxLength(500);

        // Configure hierarchical relationship (ParentBrand - ChildBrands)
        builder.HasOne(b => b.ParentBrand)
            .WithMany(b => b.ChildBrands)
            .HasForeignKey(b => b.ParentBrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
