using ShopTemplate.DB.Models;

namespace ShopTemplate.DB.Repository.Interfaces;

public interface IUserRepository
{
    Task<User?> GetById(int id);
    Task<User?> GetByUsername(string username);
    Task<IEnumerable<User>> GetAll();
    Task Add(User user);
    Task Update(User user);
    Task Delete(User user);
}