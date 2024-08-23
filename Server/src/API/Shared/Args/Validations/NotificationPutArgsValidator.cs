using FluentValidation;

namespace RTN.API.Shared.Args.Validations;

public class NotificationPutArgsValidator : AbstractValidator<NotificationPutArgs>
{
    public NotificationPutArgsValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.");

        RuleFor(x => x.IsRead)
            .NotNull()
            .WithMessage("IsRead is required.");
    }
}