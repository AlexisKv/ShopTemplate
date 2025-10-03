using ShopTemplate.DB.Models;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Dto.Requests;

namespace ShopTemplate.Mapping;

public static  class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product entity)
    {
        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Description = entity.Description,
            ImageLink = entity.ImageLink
        };
    }
    
    public static Product ToEntity(this ProductDto entity)
    {
        return new Product
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Description = entity.Description,
            ImageLink = entity.ImageLink
        };
    }
    
    public static Product ToEntity(this NewProductRequest dto, string imageLink)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            ImageLink = imageLink
        };
    }
}