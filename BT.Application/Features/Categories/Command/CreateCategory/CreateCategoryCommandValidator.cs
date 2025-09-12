using BT.Domain.Models.Common;
using FluentValidation;

namespace BT.Application.Features.Categories.Command.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name cannot be empty")
            .NotNull().WithMessage("Category name cannot be null")
            .MaximumLength(255).WithMessage("Category name must not exceed 255 characters");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Category description must not exceed 500 characters");
    }
}