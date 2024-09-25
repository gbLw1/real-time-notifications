using System.Text.Json.Serialization;

namespace RTN.API.Shared.Models;

public record NotificationModel(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("redirectUrl")] string? RedirectUrl,
    [property: JsonPropertyName("isRead")] bool IsRead);
