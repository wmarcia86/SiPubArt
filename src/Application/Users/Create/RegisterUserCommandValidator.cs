using FluentValidation;

namespace Application.Users.Create;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.FirstName)
             .NotEmpty()
             .MinimumLength(2)
             .MaximumLength(50)
             .WithName("First Name");

        RuleFor(r => r.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithName("Last Name");

        RuleFor(r => r.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .WithName("Username");

        RuleFor(r => r.Email)
             .NotEmpty()
             .MinimumLength(5)
             .MaximumLength(254)
             .WithName("Email");

        RuleFor(r => r.Password)
             .NotEmpty()
             .MinimumLength(8)
             .MaximumLength(50)
             .WithName("Password");

    }
}