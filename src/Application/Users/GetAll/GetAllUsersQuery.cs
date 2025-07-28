using Application.Users.Common;
using ErrorOr;
using MediatR;

namespace Application.Users.GetAll;

public record GetAllUsersQuery() : IRequest<ErrorOr<IReadOnlyList<UserResponse>>>;