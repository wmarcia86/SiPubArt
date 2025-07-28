using ErrorOr;
using MediatR;

namespace Application.Users.Update;

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password,
    string Role,
    bool Active) : IRequest<ErrorOr<Guid>>;
