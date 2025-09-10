using BT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.Infrastructure.Persistence.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(pv => pv.Id);
        builder.Property(pv => pv.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(pv => pv.Price)
            .IsRequired();
        builder.Property(pv => pv.Currency)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(pv => pv.Stock)
            .IsRequired();
        builder.Property(pv => pv.IsActive)
            .IsRequired();
        builder.HasOne(pv => pv.Product)
            .WithMany(p => p.ProductVariants)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}