using System.Text.Json.Serialization;

using FluentValidation;

namespace RTN.API.Shared.Args;

public class NotificationPutArgs {
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    [JsonPropertyName("isRead")]
    public bool IsRead { get; set; }

    public class Validator : AbstractValidator<NotificationPutArgs> {
        public Validator() {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(250)
                .WithMessage("Content must not exceed 250 characters.");

            RuleFor(x => x.RedirectUrl)
                .MaximumLength(250)
                .WithMessage("Redirect URL must not exceed 250 characters.");

            RuleFor(x => x.IsRead)
                .NotNull()
                .WithMessage("IsRead is required.");
        }
    }
}
