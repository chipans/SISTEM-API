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
            CreatedAt = DateTime.UtcNow,
            RevokedAt = refreshToken.RevokedAt
        };

        await _context.RefreshToken.AddAsync(entity);
        await _context.SaveChangesAsync();

        return ToModel(entity);
    }

    public async Task<RefreshTokenModel?> UpdateAsync(RefreshTokenModel refreshToken)
    {
        var entity = await _context.RefreshToken.FirstOrDefaultAsync(rt => rt.Id == refreshToken.Id);
        if (entity is null) return null;

        entity.TokenHash = refreshToken.TokenHash;
        entity.ExpiresAt = refreshToken.ExpiresAt;
        entity.RevokedAt = refreshToken.RevokedAt;

        await _context.SaveChangesAsync();
        return ToModel(entity);
    }

    public async Task RevokeAllByUserIdAsync(int userId, DateTime revokedAt)
    {
        await _context.RefreshToken
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(rt => rt.RevokedAt, revokedAt));
    }


    private static RefreshTokenModel ToModel(RefreshTokenEntity entity) =>
        new(entity.Id, entity.UserId, entity.TokenHash, entity.ExpiresAt, entity.CreatedAt, entity.RevokedAt);
}