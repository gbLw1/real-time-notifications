using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RTN.API.Data;
using RTN.API.Data.Entities;
using RTN.API.Shared.Models;

namespace RTN.API.Shared.Extensions;

public static class HttpRequestExtensions
{
    public record AuthTokenModel(Guid AuthToken, Guid UserId);
    public static async Task<AuthTokenModel> GetAuthTokenAsync(this HttpRequest request, MyDbContext dbContext)
    {
        if (!request.Headers.TryGetValue("Authorization", out var authHeaderValues))
        {
            throw new UnauthorizedAccessException("Authentication is required.");
        }

        var authToken = authHeaderValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authToken))
        {
            throw new UnauthorizedAccessException("Authentication is required.");
        }

        if (!Guid.TryParse(authToken, out var guidToken))
        {
            throw new UnauthorizedAccessException("Token format is invalid.");
        }

        var login = await dbContext
            .Set<LoginEntity>()
            .FirstOrDefaultAsync(l => l.AuthToken == guidToken && l.AuthTokenExpiryTime > DateTime.UtcNow)
            ?? throw new UnauthorizedAccessException("Token is invalid or expired.");

        return new AuthTokenModel(guidToken, login.UserId);
    }

    public class NotificationMessage(NotificationModel message, Guid? roomId)
    {
        [JsonPropertyName("message")]
        public NotificationModel Message { get; set; } = message;
        [JsonPropertyName("roomId")]
        public Guid? RoomId { get; set; } = roomId;
    }
    public static async Task SendNotificationAsync(NotificationModel message, Guid? roomId = null)
    {
        NotificationMessage bodyObj = new(message, roomId);

        if (roomId is not null)
        {
            bodyObj.RoomId = roomId;
        }

        var jsonContent = JsonSerializer.Serialize(bodyObj);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync("http://localhost:3069/send-notification", content);
        response.EnsureSuccessStatusCode();
    }
}
