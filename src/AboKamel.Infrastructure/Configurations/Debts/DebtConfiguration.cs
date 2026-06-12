using AboKamel.Domain.Entities.Debts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AboKamel.Infrastructure.Configurations.Debts;

public class DebtConfiguration : IEntityTypeConfiguration<Debt>
{
    public void Configure(EntityTypeBuilder<Debt> builder)
    {
        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

        builder.Property(x => x.DebitCredit)
            .IsRequired();

        builder.HasOne(d => d.Customer)
            .WithMany()
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}