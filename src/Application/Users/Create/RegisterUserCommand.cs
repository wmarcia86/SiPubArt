using ErrorOr;
using MediatR;

namespace Application.Users.Create;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password
) : IRequest<ErrorOr<Unit>>;