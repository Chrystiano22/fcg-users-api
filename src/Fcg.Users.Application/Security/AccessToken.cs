namespace Fcg.Users.Application.Security;

public sealed record AccessToken(
    string Value,
    DateTime ExpiresAtUtc);
