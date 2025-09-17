using BT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BT.Infrastructure.Persistence.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(n => n.Content)
            .IsRequired();
        builder.Property(n => n.ImageUrl)
            .IsRequired();
        builder.Property(n => n.IsActive)
            .IsRequired();
    }
}