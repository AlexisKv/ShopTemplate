using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.DTO;
using ShopTemplate.Helpers;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

public class ProductController  : BaseApiController
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("add-product")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> AddProduct([FromForm] NewProductDto productDto)
    {
        if (!ModelState.IsValid)
            return ValidationErrorResponse<ProductDto>();
        
        var result = await _productService.AddProduct(productDto);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }
    
    [HttpGet("get-all-products")]
    public async Task<ActionResult<PagedResult<List<ProductDto>>>> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _productService.GetAllProducts(pageNumber, pageSize);
        return Ok(response);
    }

}