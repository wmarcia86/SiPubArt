using Application.Comments.Common;
using ErrorOr;
using MediatR;

namespace Application.Comments.GetByArticleId;

public record GetByArticleIdQuery(Guid ArticleId) : IRequest<ErrorOr<IReadOnlyList<CommentResponse>>>;
