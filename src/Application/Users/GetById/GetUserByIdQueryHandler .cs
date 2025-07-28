using Application.Users.Common;
using AutoMapper;
using Domain.Users;
using ErrorOr;
using MediatR;

namespace Application.Users.GetById;

internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByIdAsync(new UserId(query.Id)) is not User user)
        {
            return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
        }

        var userResponse = _mapper.Map<UserResponse>(user);

        return userResponse;
    }
}