using FluentValidation;

namespace Application.Users.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithName("Id");
    }
}
