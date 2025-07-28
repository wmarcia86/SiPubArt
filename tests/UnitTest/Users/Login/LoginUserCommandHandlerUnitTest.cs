using Application.Common.Authentication;
using Application.Users.Login;
using Domain.Users;
using Domain.Users.ValueObjects;
using Moq;

namespace UnitTest.Users.Login;

/// <summary>
/// Unit test class for testing the LoginUserCommandHandler logic.
/// Type: Unit Test
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class LoginUserCommandHandlerUnitTest
{
    private readonly Mock<IUserRepository> _mokUserRepository;
    private readonly Mock<ITokenGenerator> _mokTokenGenerator;

    private readonly LoginUserCommandHandler _handler;

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the <see cref="LoginUserCommandHandlerUnitTest"/> class.
    /// </summary>
    public LoginUserCommandHandlerUnitTest()
    {
        _mokUserRepository = new Mock<IUserRepository>();
        _mokTokenGenerator = new Mock<ITokenGenerator>();

        _handler = new LoginUserCommandHandler(_mokUserRepository.Object, _mokTokenGenerator.Object);
    }

    /// <summary>
    /// Tests successful login with valid username and password.
    /// </summary>
    [Fact]
    public async Task HandleLoginUser_Success()
    {
        // Arrange
        var inputUsername = new Username("testuser");
        var inputPassword = new Password("TestPassword123!");

        var userId = new UserId(Guid.NewGuid());
        var username = new Username("testuser");
        var email = new Email("testuser@email.com");
        var hashedPassword = new Password(Password.Hash("TestPassword123!"));
        var firstName = new FirstName("Test");
        var lastName = new LastName("User");
        var role = UserRole.User;

        var command = new LoginUserCommand(inputUsername.Value, inputPassword.Value);
        
        var user = new User(userId, firstName, lastName, username, email, hashedPassword, role, true);

        _mokUserRepository
            .Setup(r => r.GetByUsernameAsync(It.Is<Username>(u => u.Value == username.Value)))
            .ReturnsAsync(user);

        Assert.True(user.Password.Verify(inputPassword.Value));

        _mokTokenGenerator
            .Setup(t => t.GenerateToken(It.Is<User>(u => u.Username == username)))
            .Returns("mocked-jwt-token");

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(!result.IsError);
        
        var response = result.Value;
        
        Assert.Equal(user.Id.Value, response.Id);
        Assert.Equal(user.Username.Value, response.Username);
        Assert.Equal(user.Email.Value, response.Email);
        Assert.Equal(user.FullName, response.FullName);
        Assert.Equal(user.Role.ToString(), response.Role);
        Assert.Equal("mocked-jwt-token", response.Token);
    }

    /// <summary>
    /// Tests login failure when the username is invalid.
    /// </summary>
    [Fact]
    public async Task HandleLoginUser_Fails_WhenUsernameIsInvalid()
    {
        // Arrange
        var inputUsername = new Username("invaliduser");
        var inputPassword = new Password("TestPassword123!");

        var command = new LoginUserCommand(inputUsername.Value, inputPassword.Value);

        // Simula que el usuario no existe en el repositorio
        _mokUserRepository
            .Setup(r => r.GetByUsernameAsync(It.Is<Username>(u => u.Value == inputUsername.Value)))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Code == "User.Unauthorized");
    }

    /// <summary>
    /// Tests login failure when the password is invalid.
    /// </summary>
    [Fact]
    public async Task HandleLoginUser_Fails_WhenPasswordIsInvalid()
    {
        // Arrange
        var inputUsername = new Username("testuser");
        var inputPassword = new Password("WrongPassword!");

        var userId = new UserId(Guid.NewGuid());
        var username = new Username("testuser");
        var email = new Email("testuser@email.com");
        var hashedPassword = new Password(Password.Hash("TestPassword123!"));
        var firstName = new FirstName("Test");
        var lastName = new LastName("User");
        var role = UserRole.User;

        var command = new LoginUserCommand(inputUsername.Value, inputPassword.Value);

        var user = new User(userId, firstName, lastName, username, email, hashedPassword, role, true);

        _mokUserRepository
            .Setup(r => r.GetByUsernameAsync(It.Is<Username>(u => u.Value == username.Value)))
            .ReturnsAsync(user);

        Assert.False(user.Password.Verify(inputPassword.Value));

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Code == "User.Unauthorized");
    }

}
