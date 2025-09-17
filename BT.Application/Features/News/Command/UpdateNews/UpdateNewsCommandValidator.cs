using FluentValidation;

namespace BT.Application.Features.News.Command.UpdateNews;

public class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
{
    private static readonly string[] _allowedImageExtensions = new[]
    {
        ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
    };
    public UpdateNewsCommandValidator()
    {
        RuleFor(x => x.NewsId)
            .NotEmpty().WithMessage("News ID must not be empty")
            .NotNull().WithMessage("News ID must not be null")
            .NotEqual(Guid.Empty).WithMessage("News ID must not be an empty GUID");
        RuleFor(x => x.Title)
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));
        RuleFor(x => x.Image)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return _allowedImageExtensions.Contains(extension);
            }).WithMessage("News image has an invalid file type. Allowed types are: " + string.Join(", ", _allowedImageExtensions))
            .When(x => x.Image != null);
    }
}