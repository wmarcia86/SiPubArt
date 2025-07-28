using Application.Comments.GetByArticleId;
using FluentValidation;

namespace Application.Comments.GetByArticleId;

public class GetByArticleIdQueryValidator : AbstractValidator<GetByArticleIdQuery>
{
    public GetByArticleIdQueryValidator()
    {
        RuleFor(r => r.ArticleId)
            .NotEmpty()
            .WithName("ArticleId");
    }
}
