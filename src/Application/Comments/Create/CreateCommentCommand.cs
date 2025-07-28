using ErrorOr;
using MediatR;

namespace Application.Comments.Create;

public record CreateCommentCommand(
    Guid ArticleId,
    string Content,
    Guid AuthorId
) : IRequest<ErrorOr<Guid>>;