using System.Security.Cryptography;
using System.Text;
using SistemApi.Domain.Services;

namespace SistemApi.Infrastructure.Security;

public class TokenHasher : ITokenHasher
{
    public string Hash(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}