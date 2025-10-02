using ShopTemplate.DB.Models;

namespace ShopTemplate.DB.Repository.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<Product?> GetByName(string name);
    Task Add(Product product);
    Task Update(Product product);
    Task Delete(Product product);
    IQueryable<Product> GetAllQueryable();
}