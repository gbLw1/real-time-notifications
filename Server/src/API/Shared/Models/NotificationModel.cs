using System.Text.Json.Serialization;

namespace RTN.API.Shared.Models;

public class NotificationModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("content")]
    public required string Content { get; set; }
    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }
    [JsonPropertyName("isRead")]
    public required bool IsRead { get; set; }
}