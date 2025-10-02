namespace ShopTemplate.Dto.Requests;

public class RemoveItemFromCartRequest
{
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
}