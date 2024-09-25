namespace RTN.API.Data.Entities;

public class LoginEntity : BaseEntity {
    public required string PasswordHash { get; set; }
    public required Guid? RefreshToken { get; set; }
    public required DateTime? RefreshTokenExpirationTime { get; set; }

    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
