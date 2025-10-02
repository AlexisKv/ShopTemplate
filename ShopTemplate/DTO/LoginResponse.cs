using ShopTemplate.Dto;
using ShopTemplate.Dto.Dto;

namespace ShopTemplate.DTO;

public class LoginResponse
{
    public string? JwtToken { get; set; }
    public UserDto? User { get; set; }
}