using ShopTemplate.DB.Models;

namespace ShopTemplate.Dto.Dto;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public Role Role { get; set; }
    
    public User ToDbUser(string passwordHash, string salt)
    {
        return new User()
        {
            Id = this.UserId,
            Username = this.Username,
            CreatedAt = this.CreatedAt,
            Role = this.Role,
            PasswordHash = passwordHash,
            Salt = salt
        };
    }
}