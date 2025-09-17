using BT.Domain.Enums;

namespace BT.Domain.Models.Transactions;

public class GetTransactionResponse
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public ETransactionStatus Status { get; set; }
    public string? TransactionReference { get; set; }
}