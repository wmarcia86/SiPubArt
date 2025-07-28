using Application.Articles.Create;
using FluentValidation;

namespace Application.Comments.Create;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(r => r.ArticleId)
            .NotEmpty()
            .WithName("ArticleId");

        RuleFor(r => r.Content)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(1000)
            .WithName("Content");

        RuleFor(r => r.AuthorId)
            .NotEmpty()
            .WithName("AuthorId");
    }
}