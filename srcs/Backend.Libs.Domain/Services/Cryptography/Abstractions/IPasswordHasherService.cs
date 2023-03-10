namespace Backend.Libs.Domain.Services.Cryptography.Abstractions;

public interface IPasswordHasherService
{
    string GenerateRandomSalt();
    string HashPassword(string password, string salt);
}