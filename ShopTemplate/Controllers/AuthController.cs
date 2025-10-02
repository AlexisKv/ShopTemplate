using Microsoft.AspNetCore.Mvc;
using ShopTemplate.DTO;
using ShopTemplate.Dto.Requests;
using ShopTemplate.Helpers;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

public class AuthController : BaseApiController
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] UserRequest userDto)
    {
        var result = await _userService.CreateUserAsync(userDto);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] UserRequest userDto)
    {
        var result = await _userService.LoginAsync(userDto);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }
}