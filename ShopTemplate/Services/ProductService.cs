using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopTemplate.DB;
using ShopTemplate.DB.Models;
using ShopTemplate.DTO;
using ShopTemplate.Helpers;

namespace ShopTemplate.Services;

public class ProductService
{
    private readonly ShopContext _context;
    private readonly IMapper _mapper;

    public ProductService(ShopContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductDto>> AddProduct(NewProductDto productDto)
    {
        var alreadyExists = _context.Products
            .Any(x => x.Name.ToLower() == productDto.Name.ToLower());

        if (alreadyExists)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                ErrorMessage = ["Product with this name already exists"],
            };
        }
        
        var newProduct = _mapper.Map<Product>(productDto);
        
        if (productDto.ImageData != null && productDto.ImageData.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(productDto.ImageData.FileName)}";
            var filePath = Path.Combine("Images", "Products", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productDto.ImageData.CopyToAsync(stream);
            }
            
            newProduct.ImageLink = $"/Images/Products/{fileName}";
        }
        else
        {
            newProduct.ImageLink = "/Images/Products/placeholder.png";
        }

        
        _context.Products.Add(newProduct);
        
        await _context.SaveChangesAsync();
        
        return new ApiResponse<ProductDto>
        {
            Success = true,
            Data = _mapper.Map<ProductDto>(newProduct)
        };
    }
    
    public async Task<ApiResponse<PagedResult<ProductDto>>> GetAllProducts(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Products.AsQueryable();
        var totalCount = await query.CountAsync();
        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ApiResponse<PagedResult<ProductDto>>
        {
            Success = true,
            Data = new PagedResult<ProductDto>
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Items = _mapper.Map<List<ProductDto>>(products),
            }
        };
    }

}