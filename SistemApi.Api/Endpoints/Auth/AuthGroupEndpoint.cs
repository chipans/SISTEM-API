using SistemApi.Api.Endpoints.Common;
using SistemApi.Application.Dto.Auth;
using SistemApi.Application.Services.Auth;

namespace SistemApi.Api.Endpoints.Auth;

public static class AuthGroupEndpoint
{
    public static void MapAuthGroupEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/auth").WithTags("Auth");

        group.MapPost("/register", (RegisterRequestDto request, AuthService service) =>
            service.RegisterAsync(request).ToApiResult());

        group.MapPost("/login", (LoginRequestDto request, AuthService service) =>
            service.LoginAsync(request).ToApiResult());

        group.MapPost("/google", (GoogleLoginRequestDto request, AuthService service) =>
            service.LoginWithGoogleAsync(request).ToApiResult());
    }
}
