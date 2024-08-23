using System.Text.Json.Serialization;

namespace RTN.API.Shared.Args;

public class NotificationPutArgs
{
    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }

    [JsonPropertyName("isRead")]
    public bool IsRead { get; set; }
}