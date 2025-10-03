using System.Security.Cryptography;
using Moq;
using ShopTemplate.DB.Models;
using ShopTemplate.DB.Repository.Interfaces;
using ShopTemplate.Dto.Requests;
using ShopTemplate.ResponseTypes;
using ShopTemplate.Services;

namespace ShopTemplate.Tests;

public class AuthServiceTests
{
     private readonly Mock<IUserRepository> _userRepoMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _authService = new AuthService(_userRepoMock.Object);
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
        _userRepoMock.Setup(x => x.Add(It.IsAny<User>())).Returns(Task.CompletedTask);
        
        var result = await _authService.CreateUserAsync(request);

        // Assert
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
        var user = new User
        {
            Username = "user",
            Salt = Convert.ToBase64String(new byte[16]),
            PasswordHash = Convert.ToBase64String(new byte[32])
        };
        _userRepoMock.Setup(x => x.GetByUsername(request.Username)).ReturnsAsync(user);

        var result = await _authService.LoginAsync(request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Metadata["Type"].Equals(FailureTypes.InvalidPassword));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsCorrect()
    {
        var password = "pass";
        var saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 101, HashAlgorithmName.SHA256);
        var hash = Convert.ToBase64String(pbkdf2.GetBytes(32));

        var user = new User { Username = "user", Salt = salt, PasswordHash = hash };
        _userRepoMock.Setup(x => x.GetByUsername("user")).ReturnsAsync(user);

        var request = new UserRequest { Username = "user", Password = password };
        
        var result = await _authService.LoginAsync(request);
        
        Assert.True(result.IsSuccess);
        Assert.Equal("user", result.Value.User.Username);
        Assert.NotNull(result.Value.JwtToken);
    }
}