using Application.Users.Create;
using AutoMapper;
using Domain.Core;
using Domain.Users;
using Domain.Users.ValueObjects;
using ErrorOr;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Users.Update;

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ErrorOr<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<Guid>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == Guid.Empty)
        {
            return Error.Validation(nameof(command.Id), "Please provide a valid user id.");
        }

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

        if (!Enum.TryParse<UserRole>(command.Role, true, out var role) || !Enum.IsDefined(typeof(UserRole), role))
        {
            return Error.Validation(nameof(command.Role), "Please provide a valid user role.");
        }

        if (!await _userRepository.ExistsAsync(new UserId(command.Id)))
        {
            return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
        }

        var user = _mapper.Map<User>(command);

        user.Password = password;

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id.Value;
    }
}