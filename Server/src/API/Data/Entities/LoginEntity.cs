namespace RTN.API.Data.Entities;

public class LoginEntity : BaseEntity
{
    public required string PasswordHash { get; set; }
    public required string? AuthToken { get; set; }
    public DateTime? AuthTokenExpiryTime { get; set; }
    public required string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public required Guid UserId { get; set; }
    public required UserEntity User { get; set; }
}