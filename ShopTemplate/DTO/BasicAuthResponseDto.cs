using ShopTemplate.DB.Models;

namespace ShopTemplate.DTO;

public class BasicAuthResponseDto
{
    public bool Success { get; set; }
    public string? JwtToken { get; set; }
    public string? ErrorMessage { get; set; }
    public UserDto? User { get; set; }
}