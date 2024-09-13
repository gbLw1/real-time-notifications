namespace RTN.API.Shared.Security;

public record AuthTokenModel(string AccessToken, DateTime ExpiresIn, Guid RefreshToken, Guid SocketRoom);
