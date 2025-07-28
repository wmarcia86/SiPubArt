using Application.Common.Authentication;
using Domain.Users;
using Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Users.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<LoggedUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginUserCommandHandler(IUserRepository userRepository, ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
    }

    public async Task<ErrorOr<LoggedUserResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var usernameResult = Username.CreateByLogin(command.Username);

        if (usernameResult.Value is not Username username)
        {
            return usernameResult.Errors;
        }

        var passwordResult = Password.CreateByLogin(command.Password);

        if (passwordResult.Value is not Password inputPassword)
        {
            return passwordResult.Errors;
        }

        if (await _userRepository.GetByUsernameAsync(username) is not User user)
        {
            return Error.Unauthorized("User.Unauthorized", "Invalid username or password.");
        }

        if (!user.Password.Verify(inputPassword.Value))
        {
            return Error.Unauthorized("User.Unauthorized", "Invalid username or password.");
        }

        string token = _tokenGenerator.GenerateToken(user);

        var loggedUser = new LoggedUserResponse(
            user.Id.Value,
            user.FullName,
            user.Username.Value,
            user.Email.Value,
            user.Role.ToString(),
            token
        );

        return loggedUser;
    }
}