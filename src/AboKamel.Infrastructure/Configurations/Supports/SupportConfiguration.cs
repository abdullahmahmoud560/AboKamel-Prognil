using Capsula.Domain.Entities.Supports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capsula.Infrastructure.Configurations.Supports;

public class SupportConfiguration : IEntityTypeConfiguration<Support>
{
    public void Configure(EntityTypeBuilder<Support> builder)
    {
        builder.Property(s => s.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(s => s.PhoneNumber)
               .IsRequired()
               .HasMaxLength(25);

        builder.Property(s => s.Email)
               .HasMaxLength(150);

        builder.Property(s => s.Description)
               .HasMaxLength(1000);
    }
}
