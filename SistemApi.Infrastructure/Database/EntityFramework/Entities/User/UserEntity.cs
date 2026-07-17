namespace SistemApi.Infrastructure.Database.EntityFramework.Entities.User;

public class UserEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Role { get; set; }
    public bool IsActivate { get; set; }
    public DateTime CreatedAt { get; set; }
    
}