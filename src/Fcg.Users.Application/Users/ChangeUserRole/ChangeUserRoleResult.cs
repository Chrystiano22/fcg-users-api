using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Users.ChangeUserRole;

public sealed record ChangeUserRoleResult(
    Guid UserId,
    string Name,
    string Email,
    UserRole Role,
    DateTime UpdatedAt);
