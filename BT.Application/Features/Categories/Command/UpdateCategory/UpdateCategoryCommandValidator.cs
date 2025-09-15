using FluentValidation;

namespace BT.Application.Features.Categories.Command.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name cannot be empty")
            .MaximumLength(255).WithMessage("Category name must not exceed 255 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Category description cannot be empty")
            .MaximumLength(500).WithMessage("Category description must not exceed 500 characters");
    }
}