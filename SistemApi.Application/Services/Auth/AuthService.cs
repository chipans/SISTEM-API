using System.Net;
using SistemApi.Application.Dto.Auth;
using SistemApi.Domain.Commom;
using SistemApi.Domain.Models.Auth;
using SistemApi.Domain.Models.User;
using SistemApi.Domain.Repositories;
using SistemApi.Domain.Services;

namespace SistemApi.Application.Services.Auth;

public class AuthService : IAuthService
{
    private const int RefreshTokenLifetimeMinutes = 3;

    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenHasher _tokenHasher;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenHasher tokenHasher)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenHasher = tokenHasher;
    }

    public async Task<Result<AuthResultDto>> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null || !_passwordHasher.Verify(request.Password, user.Password))
            return Result<AuthResultDto>.Failure(["Email o contraseña incorrectos."], HttpStatusCode.Unauthorized);

        if (!user.IsActivate)
            return Result<AuthResultDto>.Failure(["Esta cuenta está desactivada."], HttpStatusCode.Forbidden);
        
        await _refreshTokenRepository.DeleteAllByUserIdAsync(user.Id);

        var authResult = await IssueTokensAsync(user);
        return Result<AuthResultDto>.Success(authResult);
    }

    public async Task<Result<AuthResultDto>> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result<AuthResultDto>.Failure(["Sesión no válida."], HttpStatusCode.Unauthorized);

        var tokenHash = _tokenHasher.Hash(refreshToken);
        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (storedToken is null || !storedToken.IsActive(DateTime.UtcNow))
            return Result<AuthResultDto>.Failure(["La sesión ha expirado."], HttpStatusCode.Unauthorized);

        var user = await _userRepository.GetByIdAsync(storedToken.UserId);
        if (user is null || !user.IsActivate)
            return Result<AuthResultDto>.Failure(["La sesión ha expirado."], HttpStatusCode.Unauthorized);
        
        var accessToken = _jwtTokenGenerator.GenerateToken(user);
        var newRefreshTokenPlain = _refreshTokenGenerator.GenerateToken();
        var newRefreshTokenHash = _tokenHasher.Hash(newRefreshTokenPlain);
        var newExpiresAt = DateTime.UtcNow.AddMinutes(RefreshTokenLifetimeMinutes);
        
        await _refreshTokenRepository.RotateAsync(storedToken.Id, newRefreshTokenHash, newExpiresAt);

        var authResult = new AuthResultDto(accessToken, newRefreshTokenPlain, user.Email, user.Name, user.Role.ToString());
        return Result<AuthResultDto>.Success(authResult);
    }

    public async Task<Result<bool>> LogoutAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result<bool>.Success(true);

        var tokenHash = _tokenHasher.Hash(refreshToken);
        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (storedToken is not null)
            await _refreshTokenRepository.DeleteAsync(storedToken.Id);

        return Result<bool>.Success(true);
    }

    private async Task<AuthResultDto> IssueTokensAsync(UserModel user)
    {
        var accessToken = _jwtTokenGenerator.GenerateToken(user);
        var refreshTokenPlain = _refreshTokenGenerator.GenerateToken();
        var refreshTokenHash = _tokenHasher.Hash(refreshTokenPlain);

        var refreshTokenModel = new RefreshTokenModel(
            0,
            user.Id,
            refreshTokenHash,
            DateTime.UtcNow.AddMinutes(RefreshTokenLifetimeMinutes),
            DateTime.UtcNow);

        await _refreshTokenRepository.CreateAsync(refreshTokenModel);

        return new AuthResultDto(accessToken, refreshTokenPlain, user.Email, user.Name, user.Role.ToString());
    }
}