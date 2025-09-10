using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class Category : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
}