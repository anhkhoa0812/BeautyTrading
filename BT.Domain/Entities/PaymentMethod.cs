using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class PaymentMethod : EntityBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}