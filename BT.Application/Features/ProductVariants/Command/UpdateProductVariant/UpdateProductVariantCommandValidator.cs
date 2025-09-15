using FluentValidation;

namespace BT.Application.Features.ProductVariants.Command.UpdateProductVariant;

public class UpdateProductVariantCommandValidator : AbstractValidator<UpdateProductVariantCommand>
{
    public UpdateProductVariantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Variant name must not be empty")
            .MaximumLength(255).WithMessage("Variant name must not exceed 255 characters");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Variant price must be greater than 0");
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Variant currency must not be empty")
            .MaximumLength(50).WithMessage("Variant currency must not exceed 50 characters");
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Variant stock must be greater than or equal to 0");
    }
}