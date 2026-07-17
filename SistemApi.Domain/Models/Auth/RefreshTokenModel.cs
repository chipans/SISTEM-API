namespace SistemApi.Domain.Models.Auth;

public class RefreshTokenModel
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public RefreshTokenModel(int id, int userId, string tokenHash, DateTime expiresAt, DateTime createdAt, DateTime? revokedAt = null)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("El hash del token es requerido.", nameof(tokenHash));

        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        RevokedAt = revokedAt;
    }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAt;

    public bool IsActive(DateTime utcNow) => RevokedAt is null && !IsExpired(utcNow);

    public void Revoke(DateTime utcNow) => RevokedAt = utcNow;
}