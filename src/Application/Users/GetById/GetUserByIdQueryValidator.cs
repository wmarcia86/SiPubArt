using FluentValidation;

namespace Application.Users.GetById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithName("Id"); ;
    }
}
