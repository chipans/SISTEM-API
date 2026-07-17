namespace SistemApi.Application.Dto.Auth;

public record LoginRequestDto(string Email, string Password);

public record AuthResultDto(string AccessToken, string RefreshToken, string Email, string Name, string Role);