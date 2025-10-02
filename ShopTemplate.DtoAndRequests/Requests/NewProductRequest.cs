using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShopTemplate.Dto.Requests;

public class NewProductRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Range(0.01, double.MaxValue)]
    public double Price { get; set; }
    
    public IFormFile? ImageData { get; set; }
    
}