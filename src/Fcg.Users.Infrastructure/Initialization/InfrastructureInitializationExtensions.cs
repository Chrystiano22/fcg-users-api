using Fcg.Users.Application.Security;
using Fcg.Users.Domain.Users;
using Fcg.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fcg.Users.Infrastructure.Initialization;

public static class InfrastructureInitializationExtensions
{
    public static async Task InitializeInfrastructureAsync(
        this IServiceProvider serviceProvider,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<UsersDbContext>();

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        await SeedAdministratorAsync(dbContext, services, configuration, cancellationToken);
    }

    private static async Task SeedAdministratorAsync(
        UsersDbContext dbContext,
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        var enabled = bool.TryParse(configuration["SeedData:Admin:Enabled"], out var parsedEnabled)
            ? parsedEnabled
            : true;

        if (!enabled)
        {
            return;
        }

        var name = configuration["SeedData:Admin:Name"] ?? "FCG Admin";
        var email = configuration["SeedData:Admin:Email"] ?? "admin@fcg.local";
        var password = configuration["SeedData:Admin:Password"] ?? "Admin@123";

        var normalizedEmail = Email.Create(email).Value;
        var adminExists = await dbContext.Users
            .AnyAsync(user => user.Email == normalizedEmail, cancellationToken);

        if (adminExists)
        {
            return;
        }

        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        var administrator = User.Register(
            name,
            normalizedEmail,
            password,
            passwordHasher.Hash,
            UserRole.Administrator);

        await dbContext.Users.AddAsync(administrator, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
