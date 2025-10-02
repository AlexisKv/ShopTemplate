using FluentResults;
using Microsoft.EntityFrameworkCore;
using ShopTemplate.DB;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.DTO;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Dto.Requests;
using ShopTemplate.Helpers;
using ShopTemplate.Mapping;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductDto>> AddProduct(NewProductRequest productDto)
    {
        var alreadyExists = await _productRepository.GetByName(productDto.Name) != null;

        if (alreadyExists)
        {
            return Result.Fail<ProductDto>(new Error("Product with this name already exists")
                .WithMetadata("Type", FailureTypes.AlreadyExists));
        }
        
        string productImageLink;
        
        if (productDto.ImageData != null && productDto.ImageData.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(productDto.ImageData.FileName)}";
            var filePath = Path.Combine("Images", "Products", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await productDto.ImageData.CopyToAsync(stream);
            }
            
            productImageLink = $"/Images/Products/{fileName}";
        }
        else
        {
            productImageLink = "/Images/Products/placeholder.png";
        }

        var dbProduct = productDto.ToEntity(productImageLink);

        await _productRepository.Add(dbProduct);
        
        return Result.Ok(dbProduct.ToDto());
    }
    
    public async Task<Result<PagedResult<ProductDto>>> GetAllProducts(int pageNumber = 1, int pageSize = 10)
    {
        var query = _productRepository.GetAllQueryable();

        var totalCount = await query.CountAsync();
        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productsDtos = products.Select(p => p.ToDto()).ToList();
        
        return Result.Ok(new PagedResult<ProductDto>
        {
            TotalCount = totalCount,
            PageSize = pageSize,
            PageNumber = pageNumber,
            Items = productsDtos,
        });
    }

}