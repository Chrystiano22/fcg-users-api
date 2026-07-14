using Fcg.Users.Domain.Users;

namespace Fcg.Users.Application.Security;

public interface IAccessTokenGenerator
{
    AccessToken Generate(User user);
}
