using FluentValidation;

namespace BT.Application.Features.Accounts.Command.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username must not empty")
            .NotNull().WithMessage("Username must not null")
            .MaximumLength(255).WithMessage("Username must not exceed 255 characters");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password must not empty")
            .NotNull().WithMessage("Password must not null")
            .MaximumLength(20).WithMessage("Password must not exceed 20 characters");
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName must not empty")
            .NotNull().WithMessage("FullName must not null")
            .MaximumLength(255).WithMessage("FullName must not exceed 255 characters");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address must not empty")
            .NotNull().WithMessage("Address must not null")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number must not empty")
            .NotNull().WithMessage("Phone number must not null")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");
    }
}