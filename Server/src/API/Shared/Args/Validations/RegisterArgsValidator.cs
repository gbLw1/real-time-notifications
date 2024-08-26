using FluentValidation;

namespace RTN.API.Shared.Args.Validations;

public class RegisterArgsValidator : AbstractValidator<RegisterArgs>
{
    public RegisterArgsValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must not exceed 250 characters.");

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