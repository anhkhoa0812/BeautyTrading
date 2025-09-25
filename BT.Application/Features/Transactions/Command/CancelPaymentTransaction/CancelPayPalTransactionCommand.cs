using Mediator;

namespace BT.Application.Features.Transactions.Command.CancelPaymentTransaction;

public class CancelPayPalTransactionCommand : IRequest<bool>
{
    public string TransactionReference { get; set; }
}