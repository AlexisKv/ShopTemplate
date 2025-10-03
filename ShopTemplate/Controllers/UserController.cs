using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Helpers;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var result = await _userService.GetByIdAsync(id);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }

    [HttpPut("{id}/update-profile")]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("{id}/password")]
    public async Task<ActionResult<UserDto>> ChangePassword(Guid id)
    {
        throw new NotImplementedException();
    }

}