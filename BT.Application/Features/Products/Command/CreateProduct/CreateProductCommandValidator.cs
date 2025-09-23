using FluentValidation;

namespace BT.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private static readonly string[] _allowedImageExtensions = new[]
    {
        ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
    };
    public CreateProductCommandValidator()
    {
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name must not be empty")
            .NotNull().WithMessage("Product name must not be null")
            .MaximumLength(255).WithMessage("Product name must not exceed 255 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description must not be empty")
            .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters");
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId must not be empty")
            .NotNull().WithMessage("CategoryId must not be null")
            .NotEqual(Guid.Empty).WithMessage("CategoryId must be a valid GUID");
        RuleFor(x => x.IsHasVariants)
            .NotNull().WithMessage("IsHasVariants must not be null");
        RuleFor(x => x.Voltage)
            .MaximumLength(255).WithMessage("Voltage must not exceed 255 characters");
        RuleFor(x => x.MachinePower)
            .MaximumLength(255).WithMessage("Machine Power must not exceed 255 characters");
        RuleFor(x => x.ControlMode)
            .MaximumLength(255).WithMessage("Control Mode must not exceed 255 characters");
        RuleFor(x => x.OutputFrequency)
            .MaximumLength(255).WithMessage("Output Frequency must not exceed 255 characters");
        RuleFor(x => x.OutputPower)
            .MaximumLength(255).WithMessage("Output Power must not exceed 255 characters");
        RuleFor(x => x.LedWavelength)
            .MaximumLength(255).WithMessage("Led Wavelength must not exceed 255 characters");
        RuleFor(x => x.LedOutputPower)
            .MaximumLength(255).WithMessage("Led Output Power must not exceed 255 characters");
        RuleFor(x => x.IrFrequencyConversionInfraredLight)
            .MaximumLength(255).WithMessage("IR Frequency Conversion Infrared Light must not exceed 255 characters");
        RuleFor(x => x.IrInverterInfraredOutputPower)
            .MaximumLength(255).WithMessage("IR Inverter Infrared Output Power must not exceed 255 characters");
        RuleFor(x => x.AirPumpNegativePressure)
            .MaximumLength(255).WithMessage("Air Pump Negative Pressure must not exceed 255 characters");
        RuleFor(x => x.RotaryRfHandleTorqueMachineSetWeight)
            .MaximumLength(255).WithMessage("Rotary RF Handle Torque Machine Set Weight must not exceed 255 characters");
        RuleFor(x => x.MachineNetWeight)
            .GreaterThan(0).WithMessage("Machine Net Weight must be greater than 0");
        RuleFor(x => x.MachineSize)
            .MaximumLength(255).WithMessage("Machine Size must not exceed 255 characters");
        RuleFor(x => x.PackageSize)
            .MaximumLength(255).WithMessage("Package Size must not exceed 255 characters");
        RuleFor(x => x.PackageWeight)
            .GreaterThan(0).WithMessage("Package Weight must be greater than 0");
        When(x => !x.IsHasVariants, () =>
        {
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price must not be null when product has no variants")
                .GreaterThan(0).WithMessage("Price must be greater than 0 when product has no variants");
            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency must not be empty when product has no variants")
                .NotNull().WithMessage("Currency must not be null when product has no variants")
                .MaximumLength(50).WithMessage("Currency must not exceed 50 characters when product has no variants");
            RuleFor(x => x.Stock)
                .NotNull().WithMessage("Stock must not be null when product has no variants")
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be greater than or equal to 0 when product has no variants");
        });
        When(x => x.IsHasVariants, () =>
        {
            RuleFor(x => x.Variants)
                .NotNull().WithMessage("Variants must not be null when product has variants");
            RuleForEach(x => x.Variants).SetValidator(new CreateProductVariantRequestValidator());
        });
        RuleFor(x => x.MainImage)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return _allowedImageExtensions.Contains(extension);
            }).WithMessage("Main image has an invalid file type. Allowed types are: " + string.Join(", ", _allowedImageExtensions));
        RuleFor(x => x.Video)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return extension.Equals(".mp4");
            }).WithMessage("Video has an invalid file type. Allowed types are: .mp4")
            .When(x => x.Video != null);
        RuleForEach(x => x.Images).SetValidator(new CreateProductImageRequestValidator());
        
    }
}
public class CreateProductVariantRequestValidator : AbstractValidator<CreateProductVariantRequest>
{
    public CreateProductVariantRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Variant name must not be empty")
            .NotNull().WithMessage("Variant name must not be null")
            .MaximumLength(255).WithMessage("Variant name must not exceed 255 characters");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Variant price must be greater than 0");
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Variant currency must not be empty")
            .NotNull().WithMessage("Variant currency must not be null")
            .MaximumLength(50).WithMessage("Variant currency must not exceed 50 characters");
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Variant stock must be greater than or equal to 0");
    }
}
public class CreateProductImageRequestValidator : AbstractValidator<CreateProductImageRequest>
{
    private static readonly string[] _allowedExtensions = new[]
    {
        ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
    };
    public CreateProductImageRequestValidator()
    {
        RuleFor(x => x.Image)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return _allowedExtensions.Contains(extension);
            }).WithMessage("Product image has an invalid file type. Allowed types are: " + string.Join(", ", _allowedExtensions));
    }
}