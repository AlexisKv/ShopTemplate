using ShopTemplate.DB.Models;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Dto.Requests;

namespace ShopTemplate.Mapping;

public static class UserMappingExtensions
{
    public static User ToEntity(this UserRequest request, string hashedPassword, string salt)
    {
        return new User
        {
            Username = request.Username,
            PasswordHash = hashedPassword,
            Salt = salt
        };
    }
    
    public static UserDto ToDto(this User entity)
    {
        return new UserDto
        {
          CreatedAt = entity.CreatedAt,
          Role = entity.Role,
          UserId = entity.Id,
          Username = entity.Username
        };
    }
}