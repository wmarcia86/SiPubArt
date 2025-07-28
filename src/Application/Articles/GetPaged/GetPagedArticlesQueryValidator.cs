using Application.Articles.GetAll;
using FluentValidation;

namespace Application.Articles.GetPaged;

internal class GetPagedArticlesQueryValidator : AbstractValidator<GetPagedArticlesQuery>
{
    public GetPagedArticlesQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .NotEmpty()
            .WithName("PageNumber");

        RuleFor(r => r.PageSize)
            .NotEmpty()
            .WithName("PageSize");
    }
}
