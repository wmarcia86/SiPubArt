using AutoMapper;
using Domain.Articles;
using Domain.Articles.ValueObjects;
using Domain.Core;
using ErrorOr;
using MediatR;

namespace Application.Articles.Create;

public class CreateArticleHandler : IRequestHandler<CreateArticleCommand, ErrorOr<Guid>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateArticleHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<Guid>> Handle(CreateArticleCommand command, CancellationToken cancellationToken)
    {
        var titleResult = ArticleTitle.Create(command.Title);

        if (titleResult.Value is not ArticleTitle)
        {
            return titleResult.Errors;
        }

        var contentResult = ArticleContent.Create(command.Content);

        if (contentResult.Value is not ArticleContent)
        {
            return contentResult.Errors;
        }

        if (command.AuthorId == Guid.Empty)
        {
            return Error.Validation(nameof(command.AuthorId), "Please provide a valid author id.");
        }

        var article = _mapper.Map<Article>(command);

        _articleRepository.Add(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return article.Id.Value;
    }
}