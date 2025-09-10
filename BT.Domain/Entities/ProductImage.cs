using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;
 
public class ProductImage : EntityBase<Guid>
{
    public string ImageUrl { get; set; }
    public Guid ProductId { get; set; }
    
    public virtual Product Product { get; set; }
}