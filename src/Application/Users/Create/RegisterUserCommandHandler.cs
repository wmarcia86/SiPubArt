using AutoMapper;
using Domain.Core;
using Domain.Users;
using Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Users.Create;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<Unit>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var firstNameResult = FirstName.Create(command.FirstName);

        if (firstNameResult.Value is not FirstName)
        {
            return firstNameResult.Errors;
        }

        var lastNameResult = LastName.Create(command.LastName);

        if (lastNameResult.Value is not LastName)
        {
            return lastNameResult.Errors;
        }

        var usernameResult = Username.Create(command.Username);

        if (usernameResult.Value is not Username)
        {
            return usernameResult.Errors;
        }

        var emailResult = Email.Create(command.Email);

        if (emailResult.Value is not Email)
        {
            return emailResult.Errors;
        }

        var passwordResult = Password.Create(command.Password);

        if (passwordResult.Value is not Password password)
        {
            return passwordResult.Errors;
        }

        var role = UserRole.User;

        if (await _userRepository.GetByUsernameAsync(usernameResult.Value) is not null)
        {
            return Error.Validation("User.Username.Found", "This username is already in use.");
        }

        if (await _userRepository.GetByEmailAsync(emailResult.Value) is not null)
        {
            return Error.Validation("User.Email.Found", "This email is already in use.");
        }

        var updatedCommand = new CreateUserCommand(
           command.FirstName,
           command.LastName,
           command.Username,
           command.Email,
           command.Password,
           role.ToString()
        );

        var user = _mapper.Map<User>(updatedCommand);

        user.Password = password;

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}