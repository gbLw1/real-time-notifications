using FluentValidation;

namespace RTN.API.Shared.Args.Validations;

public class LoginArgsValidator : AbstractValidator<LoginArgs>
{
    public LoginArgsValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(250)
            .WithMessage("Email must not exceed 250 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MaximumLength(250)
            .WithMessage("Password must not exceed 250 characters.");
    }
}