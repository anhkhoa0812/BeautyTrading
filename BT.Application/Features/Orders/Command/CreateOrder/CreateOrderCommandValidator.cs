using FluentValidation;

namespace BT.Application.Features.Orders.Command.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Items cannot be empty");
        
        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(x => x.ProductVariantId)
                .NotEmpty()
                .WithMessage("ProductVariantId cannot be empty");
            items.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        });
        
        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("TaxCode cannot be empty");
    }
}