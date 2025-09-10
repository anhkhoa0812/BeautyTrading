using BT.Domain.Entities.Common;
using BT.Domain.Enums;

namespace BT.Domain.Entities;

public class Transaction : EntityAuditBase<Guid>
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public ETransactionStatus Status { get; set; }
    public string? TransactionReference { get; set; }
    public Guid OrderId { get; set; }
    public Guid PaymentMethodId { get; set; }
    
    public virtual Order Order { get; set; }
    public virtual PaymentMethod PaymentMethod { get; set; }
}