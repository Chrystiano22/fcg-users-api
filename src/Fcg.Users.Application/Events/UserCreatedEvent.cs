namespace Fcg.Users.Application.Events;

public sealed record UserCreatedEvent(
    Guid UserId,
    string Name,
    string Email,
    DateTime CreatedAt);
