using ShopTemplate.DB.Repository;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Services;
using ShopTemplate.Services.Interfaces;
using ShopTemplate.Services.Utils;

namespace ShopTemplate.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<UserService>();
        services.AddScoped<ProductService>();
        services.AddScoped<CartService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}