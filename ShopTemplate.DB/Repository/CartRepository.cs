using Microsoft.EntityFrameworkCore;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;

namespace ShopTemplate.DB.Repository;

public class CartRepository : ICartRepository
{
    private readonly ShopContext _shopContext;

    public CartRepository(ShopContext shopContext)
    {
        _shopContext = shopContext;
    }

    public async Task<Cart?> GetCartByUserId(Guid userId)
    {
        return await _shopContext.Cart
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddCart(Cart cart)
    {
        await _shopContext.Cart.AddAsync(cart);
        await _shopContext.SaveChangesAsync();
    }

    public async Task UpdateCart(Cart cart)
    {
        _shopContext.Cart.Update(cart);
        await _shopContext.SaveChangesAsync();
    }
}