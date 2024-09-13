using RTN.API.Shared.Security;

namespace RTN.API.Shared.Extensions;

public static class HttpContextExtensions {
    /// <summary>
    /// Gets the authentication token from the request.
    /// </summary>
    /// <param name="httpContext"></param>
    public static AuthToken? AuthToken(this HttpContext httpContext)
        => httpContext.Items.TryGetValue(nameof(AuthToken), out var authToken)
        ? (AuthToken?)authToken
        : null;

    /// <summary>
    /// Sets the authentication token to the request.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="authToken"></param>
    public static void AuthToken(this HttpContext httpContext, AuthToken authToken) {
        if (!httpContext.Items.TryAdd(nameof(AuthToken), authToken)) {
            throw new InvalidOperationException();
        }
    }
}
