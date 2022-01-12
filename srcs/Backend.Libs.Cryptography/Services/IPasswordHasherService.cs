namespace Backend.Libs.Cryptography.Services;

public interface IPasswordHasherService
{
    string GenerateRandomSalt();
    string HashPassword(string password, string salt);
}