namespace ShopTemplate.Dto.Requests;

public class UpdateItemQuantityRequest
{
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}