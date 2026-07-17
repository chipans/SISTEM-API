namespace SistemApi.Domain.Services;

public interface IRefreshTokenGenerator
{
    string GenerateToken();
}