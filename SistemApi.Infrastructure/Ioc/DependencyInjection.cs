using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemApi.Application.Services.Auth;
using SistemApi.Application.Services.Dish;
using SistemApi.Domain.Repositories;
using SistemApi.Domain.Services;
using SistemApi.Infrastructure.Database.EntityFramework.Context;
using SistemApi.Infrastructure.Database.EntityFramework.Repositories.Dish;
using SistemApi.Infrastructure.Database.EntityFramework.Repositories.User;
using SistemApi.Infrastructure.Security;

namespace SistemApi.Infrastructure.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SistemApiDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();

        services.AddScoped<DishService>();
        services.AddScoped<AuthService>();

        return services;
    }
}
