using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemApi.Application.Services.Auth;
using SistemApi.Application.Services.User;
using SistemApi.Domain.Repositories;
using SistemApi.Domain.Services;
using SistemApi.Infrastructure.Database.EntityFramework.Context;
using SistemApi.Infrastructure.Database.EntityFramework.Repositories.Auth;
using SistemApi.Infrastructure.Database.EntityFramework.Repositories.User;
using SistemApi.Infrastructure.Security;

namespace SistemApi.Infrastructure.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SistemApiDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddSingleton<ITokenHasher, TokenHasher>();
        

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserManagementService, UserManagementService>();

        return services;
    }
}
