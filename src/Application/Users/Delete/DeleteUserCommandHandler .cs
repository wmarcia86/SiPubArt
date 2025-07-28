using Domain.Core;
using Domain.Users;
using ErrorOr;
using MediatR;

namespace Application.Users.Delete;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByIdAsync(new UserId(command.Id)) is not User user)
        {
            return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
        }

        _userRepository.Delete(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id.Value;
    }
}
