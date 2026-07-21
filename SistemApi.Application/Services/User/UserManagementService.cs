using System.Net;
using SistemApi.Application.Dto.User;
using SistemApi.Domain.Commom;
using SistemApi.Domain.Models.User;
using SistemApi.Domain.Repositories;
using SistemApi.Domain.Services;

namespace SistemApi.Application.Services.User;

public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserManagementService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<List<UserResponseDto>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return Result<List<UserResponseDto>>.Success(users.Select(ToResponseDto).ToList());
    }

    public async Task<Result<UserResponseDto>> CreateAsync(CreateUserDto request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            return Result<UserResponseDto>.Failure(["Ya existe una cuenta con este email."], HttpStatusCode.Conflict);

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new UserModel(0, request.Email, passwordHash, request.Name, request.Role, true);
        var created = await _userRepository.CreateAsync(user);

        return Result<UserResponseDto>.Success(ToResponseDto(created), HttpStatusCode.Created);
    }

    public async Task<Result<UserResponseDto>> UpdateAsync(int id, UpdateUserDto request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return Result<UserResponseDto>.Failure(["El usuario no fue encontrado."], HttpStatusCode.NotFound);

        user.UpdateProfile(request.Name, request.Email);
        var updated = await _userRepository.UpdateAsync(user);

        return Result<UserResponseDto>.Success(ToResponseDto(updated!));
    }

    public async Task<Result<UserResponseDto>> DeactivateAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return Result<UserResponseDto>.Failure(["El usuario no fue encontrado."], HttpStatusCode.NotFound);

        user.Deactivate();
        var updated = await _userRepository.UpdateAsync(user);

        await _refreshTokenRepository.DeleteAllByUserIdAsync(id);

        return Result<UserResponseDto>.Success(ToResponseDto(updated!));
    }

    public async Task<Result<UserResponseDto>> ChangeRoleAsync(int targetUserId, int currentUserId, ChangeRoleDto request)
    {
        if (targetUserId == currentUserId)
            return Result<UserResponseDto>.Failure(["No puedes cambiar tu propio rol."], HttpStatusCode.Forbidden);

        var user = await _userRepository.GetByIdAsync(targetUserId);
        if (user is null)
            return Result<UserResponseDto>.Failure(["El usuario no fue encontrado."], HttpStatusCode.NotFound);

        user.ChangeRole(request.Role);
        var updated = await _userRepository.UpdateAsync(user);

        return Result<UserResponseDto>.Success(ToResponseDto(updated!));
    }

    public async Task<Result<UserResponseDto>> ActivateAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return Result<UserResponseDto>.Failure(["El usuario no fue encontrado."], HttpStatusCode.NotFound);

        user.Activate();
        var updated = await _userRepository.UpdateAsync(user);

        return Result<UserResponseDto>.Success(ToResponseDto(updated!));
    }

    public async Task<Result<bool>> ChangePasswordAsync(int userId, ChangePasswordDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(["El usuario no fue encontrado."], HttpStatusCode.NotFound);

        if (!_passwordHasher.Verify(request.CurrentPassword, user.Password))
            return Result<bool>.Failure(["La contraseña actual no es correcta."], HttpStatusCode.Unauthorized);

        user.SetPassword(_passwordHasher.Hash(request.NewPassword));
        await _userRepository.UpdateAsync(user);

        return Result<bool>.Success(true);
    }

    private static UserResponseDto ToResponseDto(UserModel user) =>
        new(user.Id, user.Email, user.Name, user.Role, user.IsActivate, user.CreateAt);
}