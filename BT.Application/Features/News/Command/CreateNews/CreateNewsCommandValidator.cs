using FluentValidation;

namespace BT.Application.Features.News.Command.CreateNews;

public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
{
    private static readonly string[] _allowedImageExtensions = new[]
    {
        ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
    };
    public CreateNewsCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("News title must not be empty")
            .MaximumLength(255).WithMessage("News title must not exceed 255 characters");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("News content must not be empty")
            .NotNull().WithMessage("News content must not be null");
        RuleFor(x => x.Image)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return _allowedImageExtensions.Contains(extension);
            }).WithMessage("News image has an invalid file type. Allowed types are: " + string.Join(", ", _allowedImageExtensions));
    }
}