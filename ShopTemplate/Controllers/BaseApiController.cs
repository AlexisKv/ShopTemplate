using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Helpers;

namespace ShopTemplate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected ActionResult<ApiResponse<T>> ValidationErrorResponse<T>()
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return BadRequest(new ApiResponse<T>
        {
            Success = false,
            ErrorMessage = errors,
            Data = default
        });
    }
}