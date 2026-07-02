using Microsoft.EntityFrameworkCore;
using SistemApi.Domain.Models.User;
using SistemApi.Domain.Repositories;
using SistemApi.Infrastructure.Database.EntityFramework.Context;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.User;

namespace SistemApi.Infrastructure.Database.EntityFramework.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly SistemApiDbContext _context;

    public UserRepository(SistemApiDbContext context)
    {
        _context = context;
    }

    public async Task<UserModel?> GetByIdAsync(int id)
    {
        var entity = await _context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        var entity = await _context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Email == normalized);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<UserModel?> GetByGoogleIdAsync(string googleId)
    {
        var entity = await _context.User.AsNoTracking().FirstOrDefaultAsync(u => u.GoogleId == googleId);
        return entity is null ? null : ToModel(entity);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return await _context.User.AnyAsync(u => u.Email == normalized);
    }

    public async Task<UserModel> CreateAsync(UserModel user)
    {
        var entity = new UserEntity
        {
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            FullName = user.FullName,
            GoogleId = user.GoogleId,
            IsActive = user.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _context.User.AddAsync(entity);
        await _context.SaveChangesAsync();

        return ToModel(entity);
    }

    public async Task<UserModel?> UpdateAsync(UserModel user)
    {
        var entity = await _context.User.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (entity is null) return null;

        entity.Email = user.Email;
        entity.PasswordHash = user.PasswordHash;
        entity.FullName = user.FullName;
        entity.GoogleId = user.GoogleId;
        entity.IsActive = user.IsActive;

        await _context.SaveChangesAsync();
        return ToModel(entity);
    }

    private static UserModel ToModel(UserEntity entity) =>
        new(entity.Id, entity.Email, entity.PasswordHash, entity.FullName, entity.GoogleId, entity.IsActive, entity.CreatedAt);
}
