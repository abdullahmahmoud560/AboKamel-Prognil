using Capsula.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capsula.Infrastructure.Configurations.Products;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.Property(f => f.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne<Services.Domain.Entities.Users.ApplicationUser>()
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
