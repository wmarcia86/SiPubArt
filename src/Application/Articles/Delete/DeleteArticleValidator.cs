using FluentValidation;

namespace Application.Articles.Delete;

public class DeleteArticleValidator : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithName("Id");
    }
}