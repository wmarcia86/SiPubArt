using Application.Users.Common;
using AutoMapper;
using Domain.Users;
using ErrorOr;
using MediatR;

namespace Application.Users.GetAll;

internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ErrorOr<IReadOnlyList<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<UserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<User> users = await _userRepository.GetAll();

        return users.Select(user => _mapper.Map<UserResponse>(user)).ToList(); ;
    }
}
