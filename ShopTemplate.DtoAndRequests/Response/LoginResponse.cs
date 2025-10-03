using ShopTemplate.Dto.Dto;

namespace ShopTemplate.Dto.Response;

public class LoginResponse
{
    public string? JwtToken { get; set; }
    public UserDto? User { get; set; }
}