namespace RTN.API.Shared.Models;

public record NotificationModel(Guid Id, string Content, string? RedirectUrl, bool IsRead);
