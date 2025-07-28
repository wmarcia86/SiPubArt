using Application.Users.GetById;
using FluentValidation;

namespace Application.Users.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .WithName("Username");

        RuleFor(r => r.Password)
             .NotEmpty()
             .MinimumLength(8)
             .MaximumLength(50)
             .WithName("Password");
    }
}