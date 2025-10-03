using Microsoft.EntityFrameworkCore;
using ShopTemplate.DB;
using ShopTemplate.Services.Interfaces;

namespace ShopTemplate.Extensions;

public static class SeedingInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ShopContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await DbSeeder.SeedAsync(db, passwordHasher);
    }
}