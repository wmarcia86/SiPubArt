using FluentValidation;

namespace Application.Articles.Create;

public class CreateArticleValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleValidator()
    {
        RuleFor(r => r.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200)
            .WithName("Title");

        RuleFor(r => r.Content)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(10000)
            .WithName("Content");

        RuleFor(r => r.AuthorId)
            .NotEmpty()
            .WithName("AuthorId");
    }
}