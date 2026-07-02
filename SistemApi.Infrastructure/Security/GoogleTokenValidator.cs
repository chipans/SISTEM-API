using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using SistemApi.Domain.Services;

namespace SistemApi.Infrastructure.Security;

public class GoogleTokenValidator : IGoogleTokenValidator
{
    private readonly IConfiguration _configuration;

    public GoogleTokenValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<GoogleUserInfo?> ValidateAsync(string idToken)
    {
        try
        {
            var clientId = _configuration["Google:ClientId"];
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            });

            return new GoogleUserInfo(payload.Subject, payload.Email, payload.Name ?? payload.Email);
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}
