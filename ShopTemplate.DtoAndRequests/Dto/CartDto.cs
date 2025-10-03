namespace ShopTemplate.Dto.Dto;

public class CartDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public double Total { get; set; }
    public List<CartItemDto> Items { get; set; } = [];
}