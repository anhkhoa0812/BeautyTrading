using BT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(p => p.Description)
            .HasMaxLength(1000);
        builder.Property(p => p.ImageUrl)
            .IsRequired();
        builder.Property(p => p.IsHasVariants)
            .IsRequired();
        builder.Property(p => p.BannerUrl)
            .IsRequired();
        builder.Property(p => p.MachineNetWeight)
            .IsRequired();
        builder.Property(p => p.PackageWeight)
            .IsRequired();
        builder.Property(p => p.Voltage)
            .HasMaxLength(255);
        builder.Property(p => p.MachinePower)
            .HasMaxLength(255);
        builder.Property(p => p.ControlMode)
            .HasMaxLength(255);
        builder.Property(p => p.OutputFrequency)
            .HasMaxLength(255);
        builder.Property(p => p.OutputPower)
            .HasMaxLength(255);
        builder.Property(p => p.LedWavelength)
            .HasMaxLength(255);
        builder.Property(p => p.LedOutputPower)
            .HasMaxLength(255);
        builder.Property(p => p.IrFrequencyConversionInfraredLight)
            .HasMaxLength(255);
        builder.Property(p => p.IrInverterInfraredOutputPower)
            .HasMaxLength(255);
        builder.Property(p => p.AirPumpNegativePressure)
            .HasMaxLength(255);
        builder.Property(p => p.RotaryRfHandleTorqueMachineSetWeight)
            .HasMaxLength(255);
        builder.Property(p => p.MachineSize)
            .HasMaxLength(255);
        builder.Property(p => p.PackageSize)
            .HasMaxLength(255);
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}