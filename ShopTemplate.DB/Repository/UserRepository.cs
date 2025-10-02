using Microsoft.EntityFrameworkCore;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;

namespace ShopTemplate.DB.Repository;

public class UserRepository : IUserRepository
{
    private readonly ShopContext _dbContext;

    public UserRepository(ShopContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public Task<User?> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByUsername(string username)
    {
         return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());;
    }

    public Task<IEnumerable<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public Task Update(User user)
    {
        throw new NotImplementedException();
    }

    public Task Delete(User user)
    {
        throw new NotImplementedException();
    }
}