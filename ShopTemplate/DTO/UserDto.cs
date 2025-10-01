namespace ShopTemplate.DTO;

public class UserDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Role { get; set; }
}