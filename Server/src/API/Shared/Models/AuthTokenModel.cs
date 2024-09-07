namespace RTN.API.Shared.Models;

public record AuthTokenModel(Guid AuthToken, DateTime ExpiresIn, Guid RefreshToken, Guid SocketRoom);
