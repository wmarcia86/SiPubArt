using Application.Articles.GetByAuthorId;
using FluentValidation;

namespace Application.Users.GetById;

public class GetArticlesByAuthorIdQuerValidator : AbstractValidator<GetArticlesByAuthorIdQuery>
{
    public GetArticlesByAuthorIdQuerValidator()
    {
        RuleFor(r => r.AuthorId)
            .NotEmpty()
            .WithName("AuthorId");

        RuleFor(r => r.PageNumber)
            .NotEmpty()
            .WithName("PageNumber");

        RuleFor(r => r.PageSize)
            .NotEmpty()
            .WithName("PageSize");
    }
}
