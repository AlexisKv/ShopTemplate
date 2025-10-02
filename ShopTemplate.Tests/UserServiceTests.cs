using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopTemplate.DB;
using ShopTemplate.DB.Models;
using ShopTemplate.DTO;
using ShopTemplate.Dto.Requests;
using ShopTemplate.Services;

namespace ShopTemplate.Tests;

public class UserServiceTests
{
    
    [Fact]
    public async Task CreateUserAsync_ShouldReturnError_WhenUserExists()
    {
        var userDto = new UserRequest { Username = "existing", Password = "password123" };
        var users = new List<User> { new User { Username = "existing" } }.AsQueryable();

        var dbSetMock = new Mock<DbSet<User>>();
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        var contextMock = new Mock<ShopContext>();
        contextMock.Setup(c => c.Users).Returns(dbSetMock.Object);

        var mapperMock = new Mock<IMapper>();

        var service = new UserService(contextMock.Object, mapperMock.Object);

        var result = await service.CreateUserAsync(userDto);

        Assert.False(result.Success);
        Assert.Contains("User with this username already exists", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser_WhenUserDoesNotExist()
    {
        var userDto = new UserRequest { Username = "newuser", Password = "password123" };
        var users = new List<User>().AsQueryable();

        var dbSetMock = new Mock<DbSet<User>>();
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        dbSetMock.Setup(m => m.Add(It.IsAny<User>()));

        var contextMock = new Mock<ShopContext>();
        contextMock.Setup(c => c.Users).Returns(dbSetMock.Object);
        contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto());

        var service = new UserService(contextMock.Object, mapperMock.Object);

        var result = await service.CreateUserAsync(userDto);

        Assert.True(result.Success);
        Assert.NotNull(result.Data.User);
        Assert.Equal("GeneratedTokenHere", result.Data.JwtToken);
    }
}