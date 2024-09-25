using System.Text.Json.Serialization;

namespace RTN.API.Shared.Security;

public record AuthTokenModel(
    [property: JsonPropertyName("accessToken")] string AccessToken,
    [property: JsonPropertyName("expiresIn")] DateTime ExpiresIn,
    [property: JsonPropertyName("refreshToken")] Guid RefreshToken,
    [property: JsonPropertyName("socketRoom")] Guid SocketRoom);
