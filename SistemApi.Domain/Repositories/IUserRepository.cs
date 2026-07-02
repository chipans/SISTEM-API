using SistemApi.Domain.Models.User;

namespace SistemApi.Domain.Repositories;

public interface IUserRepository
{
    Task<UserModel?> GetByIdAsync(int id);
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel?> GetByGoogleIdAsync(string googleId);
    Task<bool> ExistsByEmailAsync(string email);
    Task<UserModel> CreateAsync(UserModel user);
    Task<UserModel?> UpdateAsync(UserModel user);
}
