using System.ComponentModel.DataAnnotations;

namespace ShopTemplate.Dto.Requests;

public class UserRequest
{
    [Required]
    [StringLength(20, MinimumLength = 8)]
    [RegularExpression(@"^\w+$", ErrorMessage = "Username cannot contain spaces or special characters.")]
    public string Username { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; }
}