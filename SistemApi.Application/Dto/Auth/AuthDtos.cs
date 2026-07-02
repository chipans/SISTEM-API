namespace SistemApi.Application.Dto.Auth;

public record RegisterRequestDto(string Email, string Password, string FullName);
public record LoginRequestDto(string Email, string Password);
public record GoogleLoginRequestDto(string IdToken);
public record AuthResponseDto(string Token, string Email, string FullName);
