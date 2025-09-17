using FluentValidation;

namespace BT.Application.Features.Orders.Command.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status cannot be empty");
    }
}