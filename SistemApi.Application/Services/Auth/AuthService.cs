using System.Net;
using SistemApi.Application.Dto.Auth;
using SistemApi.Domain.Commom;
using SistemApi.Domain.Models.User;
using SistemApi.Domain.Repositories;
using SistemApi.Domain.Services;

namespace SistemApi.Application.Services.Auth;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGoogleTokenValidator _googleTokenValidator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IGoogleTokenValidator googleTokenValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _googleTokenValidator = googleTokenValidator;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            return Result<AuthResponseDto>.Failure(["Ya existe una cuenta con este email."], HttpStatusCode.Conflict);

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new UserModel(0, request.Email, passwordHash, request.FullName, null, true);
        var created = await _userRepository.CreateAsync(user);

        var token = _jwtTokenGenerator.GenerateToken(created);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(token, created.Email, created.FullName), HttpStatusCode.Created);
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null || string.IsNullOrEmpty(user.PasswordHash) || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponseDto>.Failure(["Email o contraseña incorrectos."], HttpStatusCode.Unauthorized);

        if (!user.IsActive)
            return Result<AuthResponseDto>.Failure(["Esta cuenta está desactivada."], HttpStatusCode.Forbidden);

        var token = _jwtTokenGenerator.GenerateToken(user);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(token, user.Email, user.FullName));
    }

    public async Task<Result<AuthResponseDto>> LoginWithGoogleAsync(GoogleLoginRequestDto request)
    {
        var googleInfo = await _googleTokenValidator.ValidateAsync(request.IdToken);
        if (googleInfo is null)
            return Result<AuthResponseDto>.Failure(["El token de Google no es válido."], HttpStatusCode.Unauthorized);

        var user = await _userRepository.GetByGoogleIdAsync(googleInfo.GoogleId)
                   ?? await _userRepository.GetByEmailAsync(googleInfo.Email);

        if (user is null)
        {
            var newUser = new UserModel(0, googleInfo.Email, null, googleInfo.FullName, googleInfo.GoogleId, true);
            user = await _userRepository.CreateAsync(newUser);
        }
        else if (user.GoogleId is null)
        {
            user.LinkGoogleAccount(googleInfo.GoogleId);
            user = await _userRepository.UpdateAsync(user) ?? user;
        }

        if (!user.IsActive)
            return Result<AuthResponseDto>.Failure(["Esta cuenta está desactivada."], HttpStatusCode.Forbidden);

        var token = _jwtTokenGenerator.GenerateToken(user);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(token, user.Email, user.FullName));
    }
}
