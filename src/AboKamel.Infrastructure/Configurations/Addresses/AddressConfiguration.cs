using Capsula.Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capsula.Infrastructure.Configurations.Addresses;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(x => x.Region)
            .HasMaxLength(100);

        builder.Property(x => x.BuildingName)
            .HasMaxLength(150);

        builder.Property(x => x.ApartmentNumber)
            .HasMaxLength(20);

        builder.Property(x => x.FloorNumber)
            .HasMaxLength(20);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.DetailedAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DeliveryInstructions)
            .HasMaxLength(250);

        builder.Property(x => x.Latitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(x => x.Longitude)
            .HasColumnType("decimal(9,6)");
    }
}
