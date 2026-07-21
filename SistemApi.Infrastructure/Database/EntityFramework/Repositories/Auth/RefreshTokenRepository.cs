using Microsoft.EntityFrameworkCore;
using SistemApi.Domain.Models.Auth;
using SistemApi.Domain.Repositories;
using SistemApi.Infrastructure.Database.EntityFramework.Context;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.Auth;

namespace SistemApi.Infrastructure.Database.EntityFramework.Repositories.Auth;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly SistemApiDbContext _context;

    public RefreshTokenRepository(SistemApiDbContext context)
    {
        _context = context;
    }
    
    public async Task<RefreshTokenModel?> GetByTokenHashAsync(string tokenHash)
    {
        var entity = await _context.RefreshToken.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<RefreshTokenModel> CreateAsync(RefreshTokenModel refreshToken)
    {
        var entity = new RefreshTokenEntity
        {
            UserId = refreshToken.UserId,
            TokenHash = refreshToken.TokenHash,
            ExpiresAt = refreshToken.ExpiresAt,
            CreatedAt = DateTime.UtcNow
        };

        await _context.RefreshToken.AddAsync(entity);
        await _context.SaveChangesAsync();

        return ToModel(entity);
    }

    public async Task RotateAsync(int id, string newTokenHash, DateTime newExpiresAt)
    {
        await _context.RefreshToken
            .Where(rt => rt.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(rt => rt.TokenHash, newTokenHash)
                .SetProperty(rt => rt.ExpiresAt, newExpiresAt));
    }

    public async Task DeleteAsync(int id)
    {
        await _context.RefreshToken
            .Where(rt => rt.Id == id)
            .ExecuteDeleteAsync();
    }
    public async Task DeleteAllByUserIdAsync(int userId)
    {
        await _context.RefreshToken
            .Where(rt => rt.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteExpiredAsync(DateTime utcNow)
    {
        await _context.RefreshToken
            .Where(rt => rt.ExpiresAt < utcNow)
            .ExecuteDeleteAsync();
    }
    
    private static RefreshTokenModel ToModel(RefreshTokenEntity entity) =>
        new(entity.Id, entity.UserId, entity.TokenHash, entity.ExpiresAt, entity.CreatedAt, entity.RevokedAt);
}