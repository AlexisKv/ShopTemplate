using System.ComponentModel.DataAnnotations;

namespace ShopTemplate.DB.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public string Username { get; set; }
    
    [MaxLength(256)]
    public string PasswordHash { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Role Role { get; set; } = Role.Buyer;
    
    [MaxLength(128)]
    public string Salt { get; set; }
}

public enum Role
{
    Buyer,
    Seller,
    Admin 
}