using SistemApi.Infrastructure.Database.EntityFramework.Entities.User;

namespace SistemApi.Infrastructure.Database.EntityFramework.Entities.Auth;

public class RefreshTokenEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    
    public UserEntity User { get; set; } = null!;
}