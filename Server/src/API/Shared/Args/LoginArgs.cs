using System.Text.Json.Serialization;

namespace RTN.API.Shared.Args;

public class LoginArgs
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}