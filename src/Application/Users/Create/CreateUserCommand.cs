using ErrorOr;
using MediatR;

namespace Application.Users.Create;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password,
    string Role) : IRequest<ErrorOr<Guid>>;
