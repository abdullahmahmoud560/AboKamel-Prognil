using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities.Users;

namespace Services.Infrastructure.Configurations;

public class TwoFactorVerifyConfiguration : IEntityTypeConfiguration<TwoFactorVerify>
{
    public void Configure(EntityTypeBuilder<TwoFactorVerify> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(t => t.Email).IsRequired();
        builder.Property(t => t.OTPHash).IsRequired();
        builder.Property(t => t.Purpose).IsRequired();
        builder.Property(t => t.ExpirationDate).IsRequired();
    }
}
