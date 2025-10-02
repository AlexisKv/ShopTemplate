using Microsoft.EntityFrameworkCore;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;

namespace ShopTemplate.DB.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ShopContext _dbContext;

    public ProductRepository(ShopContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetById(int id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Product?> GetByName(string name)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }

    public async Task Add(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
    }

    public Task Update(Product product)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Product product)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Product> GetAllQueryable()
    {
        return _dbContext.Products.AsQueryable();
    }
}