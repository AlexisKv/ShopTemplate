using Microsoft.AspNetCore.Mvc;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Dto.Requests;
using ShopTemplate.Helpers;
using ShopTemplate.Services;

namespace ShopTemplate.Controllers;

public class CartController: BaseApiController
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }
    
    [HttpPost("add-item-to-cart")]
    public async Task<ActionResult<CartDto>> AddItemsToCart([FromBody] CartWithItemsRequest cartRequest)
    {
        var result = await _cartService.AddItemToCart(cartRequest.UserId, cartRequest.Items.ProductId, cartRequest.Items.Quantity);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }
    
    [HttpPost("remove-item-from-cart")]
    public async Task<ActionResult<CartDto>> RemoveItemFromCart([FromBody] RemoveItemFromCartRequest cartRequest)
    {
        var result = await _cartService.RemoveItemFromCart(cartRequest.UserId, cartRequest.ProductId);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }
    
    [HttpPost("update-quantity")]
    public async Task<ActionResult<CartDto>> UpdateItemQuantity([FromBody] UpdateItemQuantityRequest cartRequest)
    {
        var result = await _cartService.UpdateItemQuantity(cartRequest.UserId, cartRequest.ProductId, cartRequest.Quantity);

        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<CartDto>> GetCartByUserId(Guid userId)
    {
        var result = await _cartService.GetCartByUserId(userId);
        
        return result.IsFailed ? result.ToActionResult() : Ok(result.Value);
    }

}