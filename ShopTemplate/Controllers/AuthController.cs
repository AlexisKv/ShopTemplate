using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Dto.Requests;
using ShopTemplate.Dto.Response;
using ShopTemplate.Helpers;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] UserRequest userDto)
    {
        var result = await _authService.CreateUserAsync(userDto);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] UserRequest userDto)
    {
        var result = await _authService.LoginAsync(userDto);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }
    
    [HttpDelete("logout")]
    public async Task<ActionResult> Logout()
    {
        throw new NotImplementedException();
    }
}