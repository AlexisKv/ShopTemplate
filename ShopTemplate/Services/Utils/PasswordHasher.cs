using System.Security.Cryptography;
using ShopTemplate.Services.Interfaces;

namespace ShopTemplate.Services.Utils;

public class PasswordHasher : IPasswordHasher
{
    public (string PasswordHash, string Salt) HashPassword(string password)
    {
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        var salt = Convert.ToBase64String(saltBytes);
        var hash = ComputePasswordHash(password, saltBytes);
        return (hash, salt);
    }

    public bool VerifyPassword(string password, string salt, string hash)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var computedHash = ComputePasswordHash(password, saltBytes);
        return computedHash == hash;
    }

    private string ComputePasswordHash(string password, byte[] saltBytes)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 101, HashAlgorithmName.SHA256);
        var hashBytes = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hashBytes);
    }
}