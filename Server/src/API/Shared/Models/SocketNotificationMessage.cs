namespace RTN.API.Shared.Models;

public record SocketNotificationMessage(NotificationModel Message, Guid? RoomId);
