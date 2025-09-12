using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class Product : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsHasVariants { get; set; }

    public virtual Category Category { get; set; }
    public virtual ICollection<ProductImage>? ProductImages { get; set; } = new List<ProductImage>();
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}