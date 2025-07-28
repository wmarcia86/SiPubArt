using Application.Articles.Common;
using AutoMapper;
using Domain.Articles;
using ErrorOr;
using MediatR;

namespace Application.Articles.GetAll;

internal sealed class GetAllArticleQueryHandler : IRequestHandler<GetAllArticlesQuery, ErrorOr<IReadOnlyList<ArticleResponse>>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public GetAllArticleQueryHandler(IArticleRepository articleRepository, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<ArticleResponse>>> Handle(GetAllArticlesQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<Article> articles = await _articleRepository.GetAll();

        return articles.Select(article => _mapper.Map<ArticleResponse>(article)).ToList(); ;
    }
}
