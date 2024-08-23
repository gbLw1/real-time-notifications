using FluentValidation;

namespace RTN.API.Shared.Args.Validations;

public class NotificationPostArgsValidator : AbstractValidator<NotificationPostArgs>
{
    public NotificationPostArgsValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(250)
            .WithMessage("Content must not exceed 250 characters.");

        RuleFor(x => x.RedirectUrl)
            .MaximumLength(250)
            .WithMessage("Redirect URL must not exceed 250 characters.");
    }
}