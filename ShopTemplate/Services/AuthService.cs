using System.Security.Cryptography;
using FluentResults;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Dto.Requests;
using ShopTemplate.Dto.Response;
using ShopTemplate.Mapping;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResponse>> CreateUserAsync(UserRequest userRequest)
    {
        var existingUser = await _userRepository.GetByUsername(userRequest.Username);


        if (existingUser != null)
        {
            return Result.Fail<LoginResponse>(new Error("User with this username already exists")
                .WithMetadata("Type", FailureTypes.AlreadyExists));
        }

        var userPassword = HashPassword(userRequest.Password);

        var userEntity = userRequest.ToEntity( userPassword.PasswordHash, userPassword.Salt);

        await _userRepository.Add(userEntity);

        return Result.Ok(new LoginResponse
        {
            User = userEntity.ToDto(),
            JwtToken = "GeneratedTokenHere"
        });
    }

    public async Task<Result<LoginResponse>> LoginAsync(UserRequest userDto)
    {
        var user = await _userRepository.GetByUsername(userDto.Username);

        if (user == null)
            return Result.Fail<LoginResponse>(new Error("User with this username does not exist")
                .WithMetadata("Type", FailureTypes.NotFound));

        var saltBytes = Convert.FromBase64String(user.Salt);
        var hash = ComputePasswordHash(userDto.Password, saltBytes);

        if (hash != user.PasswordHash)
            return Result.Fail<LoginResponse>(new Error("Incorrect password")
                .WithMetadata("Type", FailureTypes.InvalidPassword));
        
        return Result.Ok(new LoginResponse
        {
            User = user.ToDto(),
            JwtToken = "GeneratedToken"
        });
    }

    private (string PasswordHash, string Salt) HashPassword(string password)
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

    private string ComputePasswordHash(string password, byte[] saltBytes)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 101, HashAlgorithmName.SHA256))
        {
            var hashBytes = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hashBytes);
        }
    }
}