using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Dto;
using ShopTemplate.DTO;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Dto.Requests;
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
    public async Task<ActionResult<ProductDto>> AddProduct([FromForm] NewProductRequest newProductRequest)
    {
        var result = await _productService.AddProduct(newProductRequest);
        
        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }
    
    [HttpGet("get-all-products")]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _productService.GetAllProducts(pageNumber, pageSize);
        
        if (result.IsFailed)
        {
            return result.ToActionResult();
        }
        
        return Ok(result.Value);
    }
}