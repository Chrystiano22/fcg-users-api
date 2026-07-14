using Fcg.Users.Application.Authentication.AuthenticateUser;
using Fcg.Users.Application.Users.ChangeUserRole;
using Fcg.Users.Application.Users.ListUsers;
using Fcg.Users.Application.Users.RegisterUser;
using Microsoft.Extensions.DependencyInjection;

namespace Fcg.Users.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
        services.AddScoped<IListUsersUseCase, ListUsersUseCase>();
        services.AddScoped<IChangeUserRoleUseCase, ChangeUserRoleUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

        return services;
    }
}
