using Fcg.Users.Application.Events;
using Fcg.Users.Application.Security;
using Fcg.Users.Domain.Common;
using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Users.RegisterUser;

public sealed class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventPublisher _eventPublisher;

    public RegisterUserUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEventPublisher? eventPublisher = null)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _eventPublisher = eventPublisher ?? new NullEventPublisher();
    }

    public async Task<RegisterUserResult> ExecuteAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var normalizedEmail = Email.Create(command.Email).Value;

        if (await _userRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
        {
            throw new DomainValidationException("Email is already registered.");
        }

        var user = User.Register(
            command.Name,
            normalizedEmail,
            command.Password,
            _passwordHasher.Hash,
            command.Role);

        await _userRepository.AddAsync(user, cancellationToken);
        await _eventPublisher.PublishAsync(
            new UserCreatedEvent(
                user.Id,
                user.Name,
                user.Email,
                user.CreatedAt),
            cancellationToken);

        return new RegisterUserResult(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.CreatedAt);
    }
}
