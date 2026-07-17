using SistemApi.Domain.Models.Auth;

namespace SistemApi.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshTokenModel?> GetByTokenHashAsync(string tokenHash);
    Task<RefreshTokenModel> CreateAsync(RefreshTokenModel refreshToken);
    Task<RefreshTokenModel?> UpdateAsync(RefreshTokenModel refreshToken);
    Task RevokeAllByUserIdAsync(int userId, DateTime revokedAt);
}