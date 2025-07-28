using Application.Articles.Common;
using ErrorOr;
using MediatR;

namespace Application.Articles.GetAll;

public record GetAllArticlesQuery() : IRequest<ErrorOr<IReadOnlyList<ArticleResponse>>>;