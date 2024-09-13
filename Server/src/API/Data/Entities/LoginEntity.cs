namespace RTN.API.Data.Entities;

public class LoginEntity : BaseEntity {
    public required string PasswordHash { get; set; }
    public required Guid? AuthToken { get; set; } // DELETE
    public required DateTime? AuthTokenExpiryTime { get; set; } // DELETE
    public required Guid? RefreshToken { get; set; }
    public required DateTime? RefreshTokenExpiryTime { get; set; } // RENAME => ExpirationTime

    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
