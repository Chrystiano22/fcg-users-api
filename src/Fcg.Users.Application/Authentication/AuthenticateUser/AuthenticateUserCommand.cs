namespace Fcg.Users.Application.Authentication.AuthenticateUser;

public sealed record AuthenticateUserCommand(
    string Email,
    string Password);
