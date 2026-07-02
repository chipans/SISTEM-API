namespace SistemApi.Domain.Services;

public record GoogleUserInfo(string GoogleId, string Email, string FullName);

public interface IGoogleTokenValidator
{
    Task<GoogleUserInfo?> ValidateAsync(string idToken);
}
