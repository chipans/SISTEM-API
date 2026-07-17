using SistemApi.Domain.Models.User;

namespace SistemApi.Application.Dto.User;

public record CreateUserDto(string Email, string Password, string Name, RoleType Role);

public record UpdateUserDto(string Email, string Name);

public record ChangeRoleDto(RoleType Role);

public record ChangePasswordDto(string CurrentPassword, string NewPassword);

public record UserResponseDto(int Id, string Email, string Name, RoleType Role, bool IsActivate, DateTime CreateAt);