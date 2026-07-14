using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Users.RegisterUser;

public sealed record RegisterUserResult(
    Guid UserId,
    string Name,
    string Email,
    UserRole Role,
    DateTime CreatedAt);
