using System.Text.Json.Serialization;

namespace RTN.API.Shared.Args;

public class NotificationPostArgs
{
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }
}