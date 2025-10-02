using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShopTemplate.DB.Models;

namespace ShopTemplate.DB;

public class ShopContext : DbContext
{
    public ShopContext() { }

    public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Cart> Cart { get; set; }
    public virtual DbSet<CartItem> CartItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion(new EnumToStringConverter<Role>());
        
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId);
    }
}