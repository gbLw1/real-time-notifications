using System.Text.Json.Serialization;

namespace RTN.API.Shared.Models;

public record SocketNotificationMessage(
    [property: JsonPropertyName("message")] NotificationModel Message,
    [property: JsonPropertyName("roomId")] Guid? RoomId);
