using Domain.Articles;
using Domain.Core;
using ErrorOr;
using MediatR;

namespace Application.Articles.Delete;

internal sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, ErrorOr<Guid>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
    {
        if (await _articleRepository.GetByIdAsync(new ArticleId(command.Id)) is not Article article)
        {
            return Error.NotFound("Article.NotFound", "The article with the provided Id was not found.");
        }

        _articleRepository.Delete(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return article.Id.Value;
    }
}