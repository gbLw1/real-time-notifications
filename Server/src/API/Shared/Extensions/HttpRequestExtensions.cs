using Microsoft.EntityFrameworkCore;
using RTN.API.Data;
using RTN.API.Data.Entities;

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
}
