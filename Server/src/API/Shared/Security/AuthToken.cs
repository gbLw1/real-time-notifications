using System.Text.Json.Serialization;

namespace RTN.API.Shared.Security;

public record AuthToken(
    [property: JsonPropertyName("userId")] Guid UserId,
    [property: JsonPropertyName("loginId")] Guid LoginId,
    [property: JsonPropertyName("expiresIn")] DateTime ExpiresIn);
