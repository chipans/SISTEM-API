using SistemApi.Application.Dto.Auth;
using SistemApi.Domain.Commom;

namespace SistemApi.Api.Endpoints.Common;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(this Result<T> result)
    {
        return Results.Json(
            new { isSuccess = result.IsSuccess, data = result.Data, errors = result.Errors },
            statusCode: (int)result.StatusCode);
    }

    public static IResult ToAuthApiResult(this Result<AuthResultDto> result, HttpContext httpContext, string cookieName)
    {
        if (!result.IsSuccess)
        {
            return Results.Json(
                new { isSuccess = false, data = (object?)null, errors = result.Errors },
                statusCode: (int)result.StatusCode);
        }

        httpContext.Response.Cookies.Append(cookieName, result.Data!.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/api/auth"
        });

        var data = new { accessToken = result.Data.AccessToken, email = result.Data.Email, name = result.Data.Name, role = result.Data.Role };
        return Results.Json(new { isSuccess = true, data, errors = Array.Empty<string>() }, statusCode: (int)result.StatusCode);
    }
}