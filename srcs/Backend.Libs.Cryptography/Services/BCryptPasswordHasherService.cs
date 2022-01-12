namespace Backend.Libs.Cryptography.Services;

public class BCryptPasswordHasherService : IPasswordHasherService
{
    public string GenerateRandomSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt();
    }

    public string HashPassword(string password, string salt)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }
}