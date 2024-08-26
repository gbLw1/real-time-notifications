using System.Text.Json.Serialization;

namespace RTN.API.Shared.Args;

public class RegisterArgs
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}