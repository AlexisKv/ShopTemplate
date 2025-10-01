using AutoMapper;
using ShopTemplate.DB.Models;
using ShopTemplate.DTO;

namespace ShopTemplate.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Product, ProductDto>();
        CreateMap<NewProductDto, Product>()
            .ForMember(dest => dest.ImageLink, opt => opt.Ignore()); 
    }
}   