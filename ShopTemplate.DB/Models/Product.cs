using System.ComponentModel.DataAnnotations;

namespace ShopTemplate.DB.Models;

public class Product
{
    [Key]
    public int id { get; set; }
    
    [MaxLength(200)]
    public string Name { get; set; }
    
    [MaxLength(2000)]
    public string Description { get; set; }
    
    public double Price { get; set; }
    
    public string ImageLink { get; set; }
}