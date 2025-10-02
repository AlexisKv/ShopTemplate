namespace ShopTemplate.Dto.Requests;

public class NewCartItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}