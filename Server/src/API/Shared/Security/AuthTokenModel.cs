namespace RTN.API.Shared.Security;

public record AuthTokenModel(Guid AuthToken, DateTime ExpiresIn, Guid RefreshToken, Guid SocketRoom);
