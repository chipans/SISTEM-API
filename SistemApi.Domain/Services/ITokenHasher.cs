namespace SistemApi.Domain.Services;

public interface ITokenHasher
{
    string Hash(string token);
}