using Application.Articles.Common;
using ErrorOr;
using MediatR;

namespace Application.Articles.GetAll;

public record GetPagedArticlesQuery(
    int PageNumber,
    int PageSize
) : IRequest<ErrorOr<PagedArticlesResponse>>;