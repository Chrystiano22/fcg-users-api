using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Name,
    string Email,
    string Password,
    UserRole Role = UserRole.User);
