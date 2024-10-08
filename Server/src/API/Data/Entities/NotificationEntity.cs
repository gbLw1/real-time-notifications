namespace RTN.API.Data.Entities;

public class NotificationEntity : BaseEntity {
    public required string Content { get; set; }
    public string? RedirectUrl { get; set; }
    public required bool IsRead { get; set; }

    public Guid? UserId { get; set; }
    public UserEntity? User { get; set; }
}
