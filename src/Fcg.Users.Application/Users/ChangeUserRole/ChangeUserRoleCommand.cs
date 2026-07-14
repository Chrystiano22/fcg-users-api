using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Users.ChangeUserRole;

public sealed record ChangeUserRoleCommand(UserRole Role);
