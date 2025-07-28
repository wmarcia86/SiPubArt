using ErrorOr;
using MediatR;

namespace Application.Users.Delete;

public record DeleteUserCommand(Guid Id) : IRequest<ErrorOr<Guid>>;