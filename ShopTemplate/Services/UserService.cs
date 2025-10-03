using FluentResults;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Dto.Dto;
using ShopTemplate.Mapping;
using ShopTemplate.ResponseTypes;

namespace ShopTemplate.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetById(id);

        if (user == null)
            return Result.Fail<UserDto>(new Error("User with this id does not exist")
                .WithMetadata("Type", FailureTypes.NotFound));
        
        
        return Result.Ok(user.ToDto());
    }
}