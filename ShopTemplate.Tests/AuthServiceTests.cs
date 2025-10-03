using Moq;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Dto.Requests;
using ShopTemplate.ResponseTypes;
using ShopTemplate.Services;
using ShopTemplate.Services.Interfaces;

namespace ShopTemplate.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _authService = new AuthService(_userRepoMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnFail_WhenUserExists()
    {
        var request = new UserRequest { Username = "existing", Password = "pass" };
        _userRepoMock.Setup(x => x.GetByUsername(request.Username))
            .ReturnsAsync(new User { Username = request.Username });

        var result = await _authService.CreateUserAsync(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Metadata["Type"].Equals(FailureTypes.AlreadyExists));
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnSuccess_WhenUserDoesNotExist()
    {
        var request = new UserRequest { Username = "newuser", Password = "pass" };
        _userRepoMock.Setup(x => x.GetByUsername(request.Username)).ReturnsAsync((User?)null);

        _passwordHasherMock.Setup(x => x.HashPassword(request.Password))
            .Returns(("hashedpass", "salt"));

        _userRepoMock.Setup(x => x.Add(It.IsAny<User>())).Returns(Task.CompletedTask);

        var result = await _authService.CreateUserAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal("newuser", result.Value.User.Username);
        Assert.NotNull(result.Value.JwtToken);
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenUserNotFound()
    {
        var request = new UserRequest { Username = "unknown", Password = "pass" };
        _userRepoMock.Setup(x => x.GetByUsername(request.Username)).ReturnsAsync((User?)null);

        var result = await _authService.LoginAsync(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Metadata["Type"].Equals(FailureTypes.NotFound));
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenPasswordIncorrect()
    {
        var request = new UserRequest { Username = "user", Password = "wrong" };
        var user = new User { Username = "user", Salt = "salt", PasswordHash = "hash" };
        _userRepoMock.Setup(x => x.GetByUsername(request.Username)).ReturnsAsync(user);

        _passwordHasherMock.Setup(x => x.VerifyPassword(request.Password, user.Salt, user.PasswordHash))
            .Returns(false);

        var result = await _authService.LoginAsync(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Metadata["Type"].Equals(FailureTypes.InvalidPassword));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsCorrect()
    {
        var password = "pass";
        var user = new User { Username = "user", Salt = "salt", PasswordHash = "hashedpass" };
        _userRepoMock.Setup(x => x.GetByUsername("user")).ReturnsAsync(user);

        _passwordHasherMock.Setup(x => x.VerifyPassword(password, user.Salt, user.PasswordHash))
            .Returns(true);

        var request = new UserRequest { Username = "user", Password = password };

        var result = await _authService.LoginAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal("user", result.Value.User.Username);
        Assert.NotNull(result.Value.JwtToken);
    }
}
