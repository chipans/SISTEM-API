using System.Security.Cryptography;
using SistemApi.Domain.Services;

namespace SistemApi.Infrastructure.Security;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string GenerateToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}