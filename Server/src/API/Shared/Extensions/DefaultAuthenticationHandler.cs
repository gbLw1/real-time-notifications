using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using RTN.API.Services;
using RTN.API.Shared.Security;

using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RTN.API.Shared.Extensions;

public sealed class DefaultAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    CryptoService cryptoService,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder) {
    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        Logger.LogInformation("AuthenticationHandler.HandleAuthenticateAsync");

        var authorizationHeader = Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authorizationHeader)) {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var token = authorizationHeader.Split(" ").LastOrDefault();

        if (string.IsNullOrWhiteSpace(token)) {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        string? authorizationTokenPayload;
        AuthToken? authToken;
        try {
            authorizationTokenPayload = cryptoService.Decrypt(token);
            authToken = JsonSerializer.Deserialize<AuthToken>(authorizationTokenPayload);
        }
        catch (Exception) {
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }

        if (authToken is null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));

        if (authToken.ExpiresIn < DateTime.UtcNow)
            return Task.FromResult(AuthenticateResult.Fail("Token expired"));

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, authToken.UserId.ToString()),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        this.Context.AuthToken(authToken);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
