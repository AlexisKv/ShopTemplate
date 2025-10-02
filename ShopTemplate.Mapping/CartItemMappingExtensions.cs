using ShopTemplate.DB.Models;
using ShopTemplate.Dto.Dto;

namespace ShopTemplate.Mapping;

public static class CartItemMappingExtensions
{
    public static CartItemDto ToDto(this CartItem entity)
    {
        return new CartItemDto
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Quantity = entity.Quantity,
            Product = entity.Product?.ToDto()
        };
    }
    
    public static CartItem ToEntity(this CartItemDto dto)
    {
        return new CartItem
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Product = dto.Product?.ToEntity()
        };
    }
}