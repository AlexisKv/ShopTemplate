namespace ShopTemplate.Services.Interfaces;

public interface IPasswordHasher
{
    (string PasswordHash, string Salt) HashPassword(string password);
    bool VerifyPassword(string password, string hash, string salt);
}