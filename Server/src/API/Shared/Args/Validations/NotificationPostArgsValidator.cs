using FluentValidation;

namespace RTN.API.Shared.Args.Validations;

public class NotificationPostArgsValidator : AbstractValidator<NotificationPostArgs>
{
    public NotificationPostArgsValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.");
    }
}