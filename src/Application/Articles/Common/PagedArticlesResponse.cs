using Application.Articles.Common;

namespace Application.Articles.Common;

public record PagedArticlesResponse(
    int TotalCount,
    int PageNumber,
    int PageSize,
    List<ArticleResponse> Articles
);
