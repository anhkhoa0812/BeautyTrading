using FluentValidation;

namespace BT.Application.Features.Transactions.Command;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.PaymentMethodId)
            .NotEmpty().WithMessage("Payment Method is required.");
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.");
    }
}