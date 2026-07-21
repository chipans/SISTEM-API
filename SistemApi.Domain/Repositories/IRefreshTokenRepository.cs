using SistemApi.Domain.Models.Auth;

namespace SistemApi.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshTokenModel?> GetByTokenHashAsync(string tokenHash);
    Task<RefreshTokenModel> CreateAsync(RefreshTokenModel refreshToken);
    Task RotateAsync(int id, string newTokenHash, DateTime newExpiresAt);
    Task DeleteAsync(int id);
    Task DeleteAllByUserIdAsync(int userId);
    Task DeleteExpiredAsync(DateTime utcNow);
}