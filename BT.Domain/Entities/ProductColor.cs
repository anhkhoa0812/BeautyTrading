using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class ProductColor : EntityAuditBase<Guid>
{
    public string ColorName { get; set; }
    public string ImageUrl { get; set; }
    public Guid ProductId { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
}