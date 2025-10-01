using System.ComponentModel.DataAnnotations;

namespace ShopTemplate.DTO;

public class BasicUserAuthDto
{
    [Required]
    [StringLength(20, MinimumLength = 8)]
    public string Username { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; }
}