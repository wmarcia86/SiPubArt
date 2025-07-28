using Application.Articles.Common;
using AutoMapper;
using Domain.Articles;
using ErrorOr;
using MediatR;

namespace Application.Articles.GetById;

internal sealed class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ErrorOr<ArticleResponse>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public GetArticleByIdQueryHandler(IArticleRepository articleRepository, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<ArticleResponse>> Handle(GetArticleByIdQuery query, CancellationToken cancellationToken)
    {
        if (await _articleRepository.GetByIdAsync(new ArticleId(query.Id)) is not Article article)
        {
            return Error.NotFound("Article.NotFound", "The article with the provide Id was not found.");
        }

        article.Comments = article.Comments
            .OrderByDescending(c => c.PublicationDate)
            .ToList();

        var articleResponse = _mapper.Map<ArticleResponse>(article);

        return articleResponse;
    }
}