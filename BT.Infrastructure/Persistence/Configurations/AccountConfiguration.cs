using BT.Domain.Entities;
using BT.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Username)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(a => a.PasswordHash)
            .IsRequired();
        builder.Property(a => a.FullName)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(a => a.Address)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(a => a.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(a => a.Role)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (ERole)Enum.Parse(typeof(ERole), v)
            );
    }
}