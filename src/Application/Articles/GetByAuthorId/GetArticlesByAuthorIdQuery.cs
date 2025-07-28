using Application.Articles.Common;
using ErrorOr;
using MediatR;

namespace Application.Articles.GetByAuthorId;

public record GetArticlesByAuthorIdQuery(
    Guid AuthorId,
    int PageNumber,
    int PageSize) : IRequest<ErrorOr<PagedArticlesResponse>>;
