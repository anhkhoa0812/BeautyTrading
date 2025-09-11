using FluentValidation;

namespace BT.Application.Features.Authentication.Command.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username must not empty")
            .NotNull().WithMessage("Username must not null")
            .MaximumLength(255).WithMessage("Username must not exceed 255 characters");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password must not empty")
            .NotNull().WithMessage("Password must not null")
            .MaximumLength(20).WithMessage("Password must not exceed 20 characters");
    }
}