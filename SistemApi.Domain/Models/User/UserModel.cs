namespace SistemApi.Domain.Models.User;

public class UserModel
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string? PasswordHash { get; private set; }
    public string FullName { get; private set; }
    public string? GoogleId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public UserModel(int id, string email, string? passwordHash, string fullName, string? googleId, bool isActive, DateTime createdAt = default)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("El email no es válido.", nameof(email));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo es requerido.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(passwordHash) && string.IsNullOrWhiteSpace(googleId))
            throw new ArgumentException("El usuario debe tener una contraseña o una cuenta de Google vinculada.");

        Id = id;
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        FullName = fullName;
        GoogleId = googleId;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public void LinkGoogleAccount(string googleId)
    {
        if (string.IsNullOrWhiteSpace(googleId))
            throw new ArgumentException("El id de Google no es válido.", nameof(googleId));

        GoogleId = googleId;
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash de contraseña no es válido.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
