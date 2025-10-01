using Microsoft.AspNetCore.Mvc;
using ShopTemplate.DB.Models;
using ShopTemplate.DTO;
using ShopTemplate.Helpers;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

public class AuthController : BaseApiController
{
    private readonly UserService _userService;

    public AuthController (UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Register([FromBody] BasicUserAuthDto basicUserDto)
    {
        if (!ModelState.IsValid)
            return ValidationErrorResponse<LoginResponse>();
        
        var response = await _userService.CreateUserAsync(basicUserDto);
        
        if (!response.Success)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] BasicUserAuthDto basicUserDto)
    {
        if (!ModelState.IsValid)
            return ValidationErrorResponse<LoginResponse>();
        
        var response =await _userService.LoginAsync(basicUserDto);

        if (!response.Success)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }
}