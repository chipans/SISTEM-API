using Microsoft.AspNetCore.Mvc;
using SistemApi.Api.Endpoints.Common;
using SistemApi.Application.Dto.Auth;
using SistemApi.Application.Services.Auth;

namespace SistemApi.Api.Endpoints.Auth;

public static class AuthGroupEndpoint
{
    private const string RefreshTokenCookieName = "refreshToken";

    public static void MapAuthGroupEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginRequestDto request, IAuthService service, HttpContext httpContext) =>
        {
            var result = await service.LoginAsync(request);
            return result.ToAuthApiResult(httpContext, RefreshTokenCookieName);
        });

        group.MapPost("/refresh", async (IAuthService service, HttpContext httpContext) =>
        {
            var refreshToken = httpContext.Request.Cookies[RefreshTokenCookieName];
            var result = await service.RefreshAsync(refreshToken ?? string.Empty);
            return result.ToAuthApiResult(httpContext, RefreshTokenCookieName);
        });

        group.MapPost("/logout", async (IAuthService service, HttpContext httpContext) =>
        {
            var refreshToken = httpContext.Request.Cookies[RefreshTokenCookieName];
            await service.LogoutAsync(refreshToken ?? string.Empty);

            httpContext.Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/api/auth"
            });

            return Results.Ok(new { isSuccess = true });
        });
    }
}