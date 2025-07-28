using Application.Articles.Common;
using ErrorOr;
using MediatR;

namespace Application.Articles.GetById;

public record GetArticleByIdQuery(Guid Id) : IRequest<ErrorOr<ArticleResponse>>;
