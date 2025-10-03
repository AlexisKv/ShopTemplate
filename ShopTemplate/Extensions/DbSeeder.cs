using ShopTemplate.DB;
using ShopTemplate.DB.Models;
using ShopTemplate.Services.Interfaces;

namespace ShopTemplate.Extensions;

public static class DbSeeder
{
    public static async Task SeedAsync(ShopContext context, IPasswordHasher passwordHasher)
    {
        if (!context.Users.Any())
        {
            var (hash, salt) = passwordHasher.HashPassword("TestPassword");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "TestUsername",
                PasswordHash = hash,
                Salt = salt,
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Product 1", Description = "Sample product 1", Price = 10.0,
                    ImageLink = "/Images/Products/placeholder.png"
                },
                new Product
                {
                    Name = "Product 2", Description = "Sample product 2", Price = 15.5,
                    ImageLink = "/Images/Products/placeholder.png"
                },
                new Product
                {
                    Name = "Product 3", Description = "Sample product 3", Price = 20.0,
                    ImageLink = "/Images/Products/placeholder.png"
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        if (!context.Cart.Any())
        {
            var user = context.Users.First();

            var cart = new Cart
            {
                UserId = user.Id,
                Items =
                [
                    new CartItem()
                    {
                        Quantity = 2,
                        ProductId = context.Products.First().Id
                    }
                ]
            };
            
            cart.Total = cart.Items.Sum(i => i.Quantity * context.Products.First(p => p.Id == i.ProductId).Price);
            
            context.Cart.Add(cart);
            await context.SaveChangesAsync();
        }
    }
}