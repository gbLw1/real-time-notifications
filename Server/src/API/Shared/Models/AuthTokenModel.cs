using System.Text.Json.Serialization;

namespace RTN.API.Shared.Models;

public class AuthTokenModel
{
    [JsonPropertyName("authToken")]
    public required Guid AuthToken { get; set; }
    [JsonPropertyName("expiresIn")]
    public required DateTime ExpiresIn { get; set; }
    [JsonPropertyName("refreshToken")]
    public required Guid RefreshToken { get; set; }
    [JsonPropertyName("socketRoom")]
    public required Guid SocketRoom { get; set; }
}