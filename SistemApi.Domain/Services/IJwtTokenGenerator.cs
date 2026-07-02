using SistemApi.Domain.Models.User;

namespace SistemApi.Domain.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserModel user);
}
