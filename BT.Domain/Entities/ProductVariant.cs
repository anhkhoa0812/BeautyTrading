using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class ProductVariant : EntityBase<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public Guid ProductId { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
}