using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopTemplate.DB;
using ShopTemplate.DB.Models;
using ShopTemplate.DTO;
using ShopTemplate.Helpers;

namespace ShopTemplate.Services;

public class UserService
{
    private readonly ShopContext _context;
    private readonly IMapper _mapper;

    public UserService(ShopContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<LoginResponse>> CreateUserAsync(BasicUserAuthDto userDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
        
        if (existingUser != null)
        {
            return new ApiResponse<LoginResponse>
            {
                ErrorMessage = ["User with this username already exists"],
                Success = false,
                Data = new LoginResponse()
            };
        }
            
        var userPassword = HashPassword(userDto.Password);
        
        var newUser = new User()
        {
            CreatedAt = DateTime.UtcNow,
            Username = userDto.Username,
            Role = Role.Buyer,
            PasswordHash = userPassword.PasswordHash,
            Salt = userPassword.Salt
        };
        
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new ApiResponse<LoginResponse>
        {
            Success = true,
            Data = new LoginResponse()
            {
                User = _mapper.Map<UserDto>(newUser),
                JwtToken = "GeneratedTokenHere" 
            }
        };
    }
    
    public async Task<ApiResponse<LoginResponse>> LoginAsync(BasicUserAuthDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username ==  userDto.Username);
        if (user == null)
        {
            return new ApiResponse<LoginResponse>
            {
                ErrorMessage = ["User with this username does not exist"],
                Success = false,
                Data = new LoginResponse
                {
                    User = null,
                    JwtToken = ""
                }
            };
        }
        
        var saltBytes = Convert.FromBase64String(user.Salt);
        
        var hash = ComputePasswordHash(userDto.Password, saltBytes);

        if (hash == user.PasswordHash)
        {
            return  new ApiResponse<LoginResponse>
            {
                Success = true,
                Data = new LoginResponse
                {
                    User =  _mapper.Map<UserDto>(user),
                    JwtToken = "GeneratedTokenHere"
                }
            };
        }
        
        return  new ApiResponse<LoginResponse>
        {
            ErrorMessage = ["Incorrect password"],
            Success = false,
            Data = new LoginResponse()
        };
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