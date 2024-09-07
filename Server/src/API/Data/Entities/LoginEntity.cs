namespace RTN.API.Data.Entities;

public class LoginEntity : BaseEntity {
    public required string PasswordHash { get; set; }
    public required Guid? AuthToken { get; set; }
    public required DateTime? AuthTokenExpiryTime { get; set; }
    public required Guid? RefreshToken { get; set; }
    public required DateTime? RefreshTokenExpiryTime { get; set; }

    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
