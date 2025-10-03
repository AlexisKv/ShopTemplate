using FluentResults;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Dto.Requests;
using ShopTemplate.Dto.Response;
using ShopTemplate.Mapping;
using ShopTemplate.ResponseTypes;
using ShopTemplate.Services.Interfaces;
using ShopTemplate.Services.Utils;

namespace ShopTemplate.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<LoginResponse>> CreateUserAsync(UserRequest userRequest)
    {
        var existingUser = await _userRepository.GetByUsername(userRequest.Username);


        if (existingUser != null)
        {
            return Result.Fail<LoginResponse>(new Error("User with this username already exists")
                .WithMetadata("Type", FailureTypes.AlreadyExists));
        }

        var userPassword = _passwordHasher.HashPassword(userRequest.Password);

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
        
        var matchedPassword = _passwordHasher.VerifyPassword(userDto.Password, user.Salt, user.PasswordHash);

        if (!matchedPassword)
            return Result.Fail<LoginResponse>(new Error("Incorrect password")
                .WithMetadata("Type", FailureTypes.InvalidPassword));
        
        return Result.Ok(new LoginResponse
        {
            User = user.ToDto(),
            JwtToken = "GeneratedToken"
        });
    }
}