using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Transactions.Command.CreateTransaction;

public class CreateTransactionCommand : IRequest<ApiResponse>
{
    public Guid OrderId { get; set; }
    
    public Guid PaymentMethodId { get; set; }
    
    public string Currency { get; set; }
}


public class CreateTransactionDTO
{
    public Guid PaymentMethodId { get; set; }
    
    public string Currency { get; set; }
}