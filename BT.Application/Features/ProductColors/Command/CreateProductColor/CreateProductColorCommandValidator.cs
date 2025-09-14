using FluentValidation;

namespace BT.Application.Features.ProductColors.Command.CreateProductColor;

public class CreateProductColorCommandValidator : AbstractValidator<CreateProductColorCommand>
{
    private static readonly string[] _allowedExtensions = new[]
    {
        ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
    };
    public CreateProductColorCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .NotNull().WithMessage("ProductId must not be null.")
            .NotEqual(Guid.Empty).WithMessage("ProductId must be a valid GUID.");
        RuleFor(x => x.ColorName)
            .NotEmpty().WithMessage("ColorName is required.")
            .NotNull().WithMessage("ColorName must not be null.")
            .MaximumLength(50).WithMessage("ColorName must not exceed 50 characters.");
        RuleFor(x => x.Image)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return _allowedExtensions.Contains(extension);
            }).WithMessage("Product image has an invalid file type. Allowed types are: " + string.Join(", ", _allowedExtensions));
    }
}