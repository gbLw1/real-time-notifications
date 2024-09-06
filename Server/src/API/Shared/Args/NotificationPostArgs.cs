using System.Text.Json.Serialization;
using FluentValidation;

namespace RTN.API.Shared.Args;

public class NotificationPostArgs
{
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    public class Validator : AbstractValidator<NotificationPostArgs>
    {
        public Validator()
        {
            RuleFor(n => n.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(250)
                .WithMessage("Content must not exceed 250 characters.");

            RuleFor(n => n.RedirectUrl)
                .MaximumLength(250)
                .WithMessage("Redirect URL must not exceed 250 characters.");
        }
    }
}