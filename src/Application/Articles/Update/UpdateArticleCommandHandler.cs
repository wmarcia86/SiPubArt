using AutoMapper;
using Domain.Articles;
using Domain.Articles.ValueObjects;
using Domain.Core;
using Domain.Users;
using ErrorOr;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Articles.Update;

internal sealed class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ErrorOr<Guid>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<Guid>> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == Guid.Empty)
        {
            return Error.Validation(nameof(command.Id), "Please provide a valid article id.");
        }

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

        if (!await _articleRepository.ExistsAsync(new ArticleId(command.Id)))
        {
            return Error.NotFound("Article.NotFound", "The article with the provide Id was not found.");
        }

        if (await _articleRepository.GetByIdAsync(new ArticleId(command.Id)) is not Article article)
        {
            return Error.NotFound("Article.NotFound", "The article with the provide Id was not found.");
        }

        _mapper.Map(command, article);

        _articleRepository.Update(article);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return article.Id.Value;
    }
}