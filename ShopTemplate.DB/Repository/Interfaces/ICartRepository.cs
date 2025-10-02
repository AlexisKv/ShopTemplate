using ShopTemplate.DB.Models;

namespace ShopTemplate.DB.Repository.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserId(Guid userId);
    Task AddCart(Cart cart);
    Task UpdateCart(Cart cart);
}