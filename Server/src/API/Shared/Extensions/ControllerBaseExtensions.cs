using Microsoft.AspNetCore.Mvc;

using RTN.API.Shared.Security;

namespace RTN.API.Shared.Extensions;

public static class ControllerBaseExtensions {
    public static AuthToken? GetAuthToken(
        this ControllerBase controller,
        bool allowAnonymous = false
    ) {
        var authToken = controller.HttpContext.AuthToken();

        if (authToken is null && !allowAnonymous) {
            throw new UnauthorizedAccessException();
        }

        return authToken;
    }
}
