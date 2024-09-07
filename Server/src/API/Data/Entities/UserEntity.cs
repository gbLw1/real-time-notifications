namespace RTN.API.Data.Entities;

public class UserEntity : BaseEntity {
    public required string Name { get; set; }
    public required string Email { get; set; }

    public IReadOnlyCollection<LoginEntity>? Logins { get; set; }
    public IReadOnlyCollection<NotificationEntity>? Notifications { get; set; }
}
