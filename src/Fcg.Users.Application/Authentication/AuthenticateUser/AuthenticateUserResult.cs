using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Authentication.AuthenticateUser;

public sealed record AuthenticateUserResult(
    Guid UserId,
    string Name,
    string Email,
    UserRole Role,
    string Token,
    DateTime ExpiresAtUtc);
