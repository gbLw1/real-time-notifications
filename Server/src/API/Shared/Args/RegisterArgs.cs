using System.Text.Json.Serialization;

using FluentValidation;

namespace RTN.API.Shared.Args;

public class RegisterArgs {
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }

    public class Validator : AbstractValidator<RegisterArgs> {
        public Validator() {
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
}
