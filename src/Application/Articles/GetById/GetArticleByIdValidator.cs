using FluentValidation;

namespace Application.Articles.GetById;

public class GetArticleByIdValidator : AbstractValidator<GetArticleByIdQuery>
{
    public GetArticleByIdValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithName("Id");
    }
}