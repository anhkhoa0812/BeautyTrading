using BT.Domain.Entities;
using BT.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount)
            .IsRequired();
        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(t => t.Status)
            .HasConversion(
                v => v.ToString(),
                v => (ETransactionStatus)Enum.Parse(typeof(ETransactionStatus), v)
            );
        builder.Property(t => t.TransactionReference)
            .HasMaxLength(500);
        builder.HasOne(t => t.Order)
            .WithMany(o => o.Transactions)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.PaymentMethod)
            .WithMany(pm => pm.Transactions)
            .HasForeignKey(t => t.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}