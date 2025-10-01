namespace ShopTemplate.DB.Models;

public class Cart
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();
}