namespace ShopTemplate.Dto.Requests;

public class CartWithItemsRequest
{
    public Guid UserId { get; set; }
    public NewCartItemRequest Items { get; set; } = new();
}
