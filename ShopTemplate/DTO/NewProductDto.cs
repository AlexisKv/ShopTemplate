using System.ComponentModel.DataAnnotations;

namespace ShopTemplate.DTO;

public class NewProductDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Range(0.01, double.MaxValue)]
    public double Price { get; set; }
    
    public IFormFile? ImageData { get; set; }
}