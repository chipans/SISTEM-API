using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SistemApi.Api.Endpoints.Common;

public static class HttpContextExtensions
{
    public static int GetCurrentUserId(this HttpContext httpContext)
    {
        var subClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)
                       ?? httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        return int.Parse(subClaim!.Value);
    }
}