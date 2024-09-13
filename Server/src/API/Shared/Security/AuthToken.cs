namespace RTN.API.Shared.Security;

public record AuthToken(Guid UserId, Guid LoginId, DateTime ExpiresIn);
