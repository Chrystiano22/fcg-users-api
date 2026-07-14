using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Users.ListUsers;

public sealed record ListUsersResult(
    Guid UserId,
    string Name,
    string Email,
    UserRole Role,
    DateTime CreatedAt);
