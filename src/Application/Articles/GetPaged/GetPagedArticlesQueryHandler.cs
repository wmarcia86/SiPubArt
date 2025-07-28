using Application.Articles.Common;
using Application.Articles.GetAll;
using AutoMapper;
using Domain.Articles;
using ErrorOr;
using MediatR;

namespace Application.Articles.GetPaged;

internal class GetPagedArticlesQueryHandler : IRequestHandler<GetPagedArticlesQuery, ErrorOr<PagedArticlesResponse>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public GetPagedArticlesQueryHandler(IArticleRepository articleRepository, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<PagedArticlesResponse>> Handle(GetPagedArticlesQuery query, CancellationToken cancellationToken)
    {
        if (query.PageNumber <= 0)
        {
            return Error.Validation(
                code: "InvalidPageNumber",
                description: "Page number must be greater than zero."
            );
        }

        if (query.PageSize <= 0)
        {
            return Error.Validation(
                code: "InvalidPageSize",
                description: "Page size must be greater than zero."
            );
        }

        if (query.PageSize > 100)
        {
            return Error.Validation(
                code: "PageSizeTooLarge",
                description: "Page size must not exceed 100."
            );
        }

        int totalCount = await _articleRepository.Count();

        IReadOnlyList<Article> articles = await _articleRepository.GetPaged(query.PageNumber, query.PageSize);

        var articlesResponse = articles.Select(article =>
        {
            article.Content.Value = article.Content.Value.Length > 500 
                ? article.Content.Value.Substring(0, 500) + "..."
                : article.Content.Value;

            var articleResponse = _mapper.Map<ArticleResponse>(article);

            return articleResponse;
        }).ToList();

        var pagedArticlesResponse = new PagedArticlesResponse(
            TotalCount: totalCount,
            PageNumber: query.PageNumber,
            PageSize: query.PageSize,
            Articles: articlesResponse
        );

        return pagedArticlesResponse;
    }
}