using Mediator;

namespace BT.Application.Features.Transactions.Command.HandlePaymentTransaction;

public class HandlePayPalWebhookCommand : IRequest<Unit>
{
    public string Body { get; set;  }
    public IHeaderDictionary Headers { get; set; }
}