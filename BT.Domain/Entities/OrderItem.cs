using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class OrderItem : EntityBase<Guid>
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductVariantId { get; set; }
    public Guid? ProductColorId { get; set; }
    public Order Order { get; set; } 
    public ProductVariant ProductVariant { get; set; }
    public virtual ProductColor? ProductColor { get; set; }
}