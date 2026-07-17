using SistemApi.Application.Dto.Auth;
using SistemApi.Domain.Commom;

namespace SistemApi.Application.Services.Auth;

public interface IAuthService
{
    Task<Result<AuthResultDto>> LoginAsync(LoginRequestDto request);
    Task<Result<AuthResultDto>> RefreshAsync(string refreshToken);
    Task<Result<bool>> LogoutAsync(string refreshToken);
}