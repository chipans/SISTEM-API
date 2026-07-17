using SistemApi.Application.Dto.User;
using SistemApi.Domain.Commom;

namespace SistemApi.Application.Services.User;

public interface IUserManagementService
{
    Task<Result<List<UserResponseDto>>> GetAllAsync();
    Task<Result<UserResponseDto>> CreateAsync(CreateUserDto request);
    Task<Result<UserResponseDto>> UpdateAsync(int id, UpdateUserDto request);
    Task<Result<UserResponseDto>> ChangeRoleAsync(int targetUserId, int currentUserId, ChangeRoleDto request);
    Task<Result<UserResponseDto>> ActivateAsync(int id);
    Task<Result<UserResponseDto>> DeactivateAsync(int id);
    Task<Result<bool>> ChangePasswordAsync(int userId, ChangePasswordDto request);
}