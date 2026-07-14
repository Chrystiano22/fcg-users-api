namespace Fcg.Users.Application.Users.ListUsers;

public interface IListUsersUseCase
{
    Task<IReadOnlyCollection<ListUsersResult>> ExecuteAsync(
        CancellationToken cancellationToken = default);
}
