using ErrorOr;
using MediatR;

namespace Application.Users.Login;

public record LoginUserCommand(
    string Username, 
    string Password) : IRequest<ErrorOr<LoggedUserResponse>>;