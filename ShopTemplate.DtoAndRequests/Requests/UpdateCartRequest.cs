namespace ShopTemplate.Dto.Requests;

public class UpdateCartRequest
{
    public Guid UserId { get; set; }
    public List<NewCartItemRequest> Items { get; set; } = new();
}