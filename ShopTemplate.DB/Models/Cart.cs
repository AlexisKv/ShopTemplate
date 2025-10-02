using System.ComponentModel.DataAnnotations;

namespace ShopTemplate.DB.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(36)]
    public Guid UserId { get; set; }
    
    public double Total { get; set; }
    
    public List<CartItem> Items { get; set; } = [];
}