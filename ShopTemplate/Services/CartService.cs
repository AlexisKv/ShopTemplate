using FluentResults;
using Microsoft.AspNetCore.SignalR;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Hubs;
using ShopTemplate.Mapping;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHubContext<CartHub> _hubContext;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository, IHubContext<CartHub> hubContext)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _hubContext = hubContext;
    }

    public async Task<Result<CartDto>> GetCartByUserId(Guid userId)
    {
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null)
        {
            return Result.Fail<CartDto>(new Error("Cart not found")
                .WithMetadata("Type", FailureTypes.NotFound));
        }

        await NotifyCartUpdated(userId, cart.ToDto());
        return Result.Ok(cart.ToDto());
    }

    public async Task<Result> AddItemToCart(Guid userId, int productId, int quantity)
    {
        var cart = await _cartRepository.GetCartByUserId(userId) ?? new Cart
        {
            UserId = userId,
            Items = new List<CartItem>()
        };

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            item.Quantity += quantity;
        }
        else
        {
            var product = await _productRepository.GetById(productId);
            if (product == null)
                return Result.Fail(new Error("Product not found")
                    .WithMetadata("Type", FailureTypes.NotFound));

            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Product = product,
                Quantity = quantity
            });
        }

        UpdateCartTotal(cart);

        if (cart.Id == 0)
            await _cartRepository.AddCart(cart);
        else
            await _cartRepository.UpdateCart(cart);

        await NotifyCartUpdated(userId, cart.ToDto());
        return Result.Ok();
    }

    public async Task<Result> RemoveItemFromCart(Guid userId, int productId)
    {
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null) return null;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            cart.Items.Remove(item);
            UpdateCartTotal(cart);
            await _cartRepository.UpdateCart(cart);
        }

        await NotifyCartUpdated(userId, cart.ToDto());
        return Result.Ok();
    }

    public async Task<Result> UpdateItemQuantity(Guid userId, int productId, int quantity)
    {
        var cart = await _cartRepository.GetCartByUserId(userId);
        if (cart == null)
            return Result.Fail(new Error("Cart not found")
                .WithMetadata("Type", FailureTypes.NotFound));

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.Quantity = quantity;
            UpdateCartTotal(cart);
            await _cartRepository.UpdateCart(cart);
            
            await NotifyCartUpdated(userId, cart.ToDto());
            return Result.Ok();
        }
        
        
        return Result.Fail(new Error("Product not found in cart")
            .WithMetadata("Type", FailureTypes.NotFound));
        
    }
    
    private async Task NotifyCartUpdated(Guid userId, CartDto cartDto)
    {
        await _hubContext.Clients.Group(userId.ToString()).SendAsync("CartUpdated", cartDto);
    }

    //todo: refactor
    private void UpdateCartTotal(Cart cart)
    {
        cart.Total = cart.Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);
    }
}