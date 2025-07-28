using ErrorOr;
using MediatR;

namespace Application.Articles.Create;

public record CreateArticleCommand(
    string Title,
    string Content,
    Guid AuthorId
) : IRequest<ErrorOr<Guid>>;