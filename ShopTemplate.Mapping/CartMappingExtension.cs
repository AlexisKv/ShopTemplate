using ShopTemplate.DB.Models;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Dto.Requests;

namespace ShopTemplate.Mapping;

public static class CartMappingExtension
{
    public static CartDto ToDto(this Cart entity)
    {
        return new CartDto
        {
            Id = entity.Id,
            Items = entity.Items.Select(x => x.ToDto()).ToList(),
            Total = entity.Total,
            UserId = entity.UserId
        };
    }
    
    public static Cart ToEntity(this CartDto dto)
    {
        return new Cart
        {
            Id = dto.Id,
            Items = dto.Items.Select(x => x.ToEntity()).ToList(),
            Total = dto.Total,
            UserId = dto.UserId
            
        };
    }
}