using BT.Domain.Entities.Common;
using BT.Domain.Enums;

namespace BT.Domain.Entities;

public class Order : EntityAuditBase<Guid>
{
    public EOrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public Guid AccountId { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public string TaxCode { get; set; } 
    public virtual Account Account { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}